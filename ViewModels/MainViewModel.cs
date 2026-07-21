using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JobSearchApp.Data;
using JobSearchApp.Models;
using JobSearchApp.Services;
using System.Collections.ObjectModel;

namespace JobSearchApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IJobSource _jobSource;

        [ObservableProperty]
        private string appEmail = string.Empty;

        [ObservableProperty]
        private string userEmail = string.Empty;

        [ObservableProperty]
        private string connectionStatus = string.Empty;

        [ObservableProperty]
        private string? selectedProfession;

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        public ObservableCollection<string> Professions { get; } = new() { "Všechny profese" };
        public ObservableCollection<PortalOptionViewModel> Portals { get; } = new();
        public ObservableCollection<JobListing> JobListings { get; } = new();

        public string WindowTitle => ApplicationVersionService.DisplayName;

        public event Action? ResetRequested;

        public MainViewModel()
            : this(new TestJobSource())
        {
        }

        internal MainViewModel(IJobSource jobSource)
        {
            _jobSource = jobSource;
            LoadSettings();
            LoadPortals();
            SelectedProfession = Professions[0];
            RefreshCommand.Execute(null);
        }

        private void LoadSettings()
        {
            using var db = new AppDbContext();
            var settings = db.AppSettings.Find(1);
            if (settings is null) return;

            if (settings.IsDemoMode)
            {
                AppEmail = "Nepřipojeno";
                UserEmail = "Nepřipojeno";
                ConnectionStatus = "Ukázkový režim";
                return;
            }

            AppEmail = settings.AppEmail;
            UserEmail = settings.UserEmail;
            ConnectionStatus = "E-mailové připojení je nastavené";
        }

        private void LoadPortals()
        {
            using var db = new AppDbContext();
            foreach (var portal in db.Portals.OrderBy(portal => portal.Name).ToList())
            {
                var portalViewModel = new PortalOptionViewModel(portal);
                portalViewModel.SelectionChanged += SavePortalSelection;
                Portals.Add(portalViewModel);
            }
        }

        private static void SavePortalSelection(int portalId, bool isSelected)
        {
            using var db = new AppDbContext();
            var portal = db.Portals.Find(portalId);
            if (portal is null || portal.IsSelected == isSelected) return;

            portal.IsSelected = isSelected;
            db.SaveChanges();
        }

        [RelayCommand]
        private void SelectAllPortals()
        {
            foreach (var portal in Portals) portal.IsSelected = true;
        }

        [RelayCommand]
        private void SelectNonePortals()
        {
            foreach (var portal in Portals) portal.IsSelected = false;
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            StatusMessage = "Načítám nabídky…";

            try
            {
                // TODO: DEVELOPMENT DATA
                // TestJobSource později nahradí adaptéry IJobSource pro konkrétní portály.
                var allListings = await _jobSource.LoadAsync();
                var selectedPortals = Portals
                    .Where(portal => portal.IsSelected)
                    .Select(portal => portal.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var profession in allListings
                             .Select(listing => listing.Profession)
                             .Distinct()
                             .OrderBy(profession => profession))
                {
                    if (!Professions.Contains(profession))
                        Professions.Add(profession);
                }

                var filtered = allListings
                    .Where(listing => selectedPortals.Contains(listing.Portal))
                    .Where(listing => SelectedProfession is null
                                      || SelectedProfession == "Všechny profese"
                                      || listing.Profession == SelectedProfession)
                    .ToList();

                JobListings.Clear();
                foreach (var listing in filtered)
                    JobListings.Add(listing);

                StatusMessage = selectedPortals.Count == 0
                    ? "Vyber alespoň jeden pracovní portál."
                    : $"Zobrazeno {filtered.Count} nabídek · zdroj: {_jobSource.Name}";
            }
            catch (Exception exception)
            {
                StatusMessage = $"Nabídky se nepodařilo načíst: {exception.Message}";
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private void Reset()
        {
            var result = System.Windows.MessageBox.Show(
                "Opravdu chceš změnit nastavení? Uložené propojení e-mailu bude odstraněno.",
                "Změnit nastavení",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (result != System.Windows.MessageBoxResult.Yes) return;

            using var db = new AppDbContext();
            var settings = db.AppSettings.Find(1);
            if (settings is not null)
            {
                settings.IsConfigured = false;
                settings.IsDemoMode = false;
                settings.AppEmail = string.Empty;
                settings.UserEmail = string.Empty;
                settings.AppPasswordEncrypted = string.Empty;
                db.SaveChanges();
            }

            ResetRequested?.Invoke();
        }
    }
}
