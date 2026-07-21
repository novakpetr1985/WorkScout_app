using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JobSearchApp.Data;
using JobSearchApp.Models;
using JobSearchApp.Services;
using MimeKit;

namespace JobSearchApp.ViewModels
{
    public partial class SetupWizardViewModel : ObservableObject
    {
        private readonly EmailConnectionService _emailService = new();

        [ObservableProperty]
        private string appEmail = string.Empty;

        // Naplňuje se z code-behind PasswordBoxu (viz SetupWizardWindow.xaml.cs) -
        // PasswordBox.Password nejde přímo bindovat kvůli bezpečnosti.
        [ObservableProperty]
        private string appPassword = string.Empty;

        [NotifyCanExecuteChangedFor(nameof(FinishCommand))]
        [ObservableProperty]
        private string userEmail = string.Empty;

        [ObservableProperty]
        private string verificationMessage = string.Empty;

        [ObservableProperty]
        private bool isVerifying;

        [NotifyCanExecuteChangedFor(nameof(FinishCommand))]
        [ObservableProperty]
        private bool isVerified;

        [ObservableProperty]
        private bool isSaving;

        // Code-behind na tohle naváže otevření MainWindow a zavření wizardu.
        public event Action? SetupCompleted;

        partial void OnAppEmailChanged(string value) => InvalidateVerification();

        partial void OnAppPasswordChanged(string value) => InvalidateVerification();

        private void InvalidateVerification()
        {
            IsVerified = false;
            VerificationMessage = string.Empty;
        }

        [RelayCommand]
        private async Task VerifyConnectionAsync()
        {
            if (string.IsNullOrWhiteSpace(AppEmail) || string.IsNullOrWhiteSpace(AppPassword))
            {
                VerificationMessage = "Vyplň e-mail a app password.";
                return;
            }

            IsVerifying = true;
            IsVerified = false;
            VerificationMessage = "Ověřuji připojení...";

            try
            {
                AppEmail = AppEmail.Trim();
                AppPassword = AppPassword.Replace(" ", string.Empty, StringComparison.Ordinal);

                if (!MailboxAddress.TryParse(AppEmail, out _))
                {
                    VerificationMessage = "E-mail pro aplikaci nemá platný formát.";
                    return;
                }

                var result = await _emailService.VerifyConnectionAsync(AppEmail, AppPassword);
                IsVerified = result.Success;
                VerificationMessage = result.Message;
            }
            finally
            {
                IsVerifying = false;
            }
        }

        private bool CanFinish() =>
            IsVerified && !IsSaving && MailboxAddress.TryParse(UserEmail.Trim(), out _);

        [RelayCommand(CanExecute = nameof(CanFinish))]
        private async Task FinishAsync()
        {
            try
            {
                IsSaving = true;
                FinishCommand.NotifyCanExecuteChanged();
                UserEmail = UserEmail.Trim();

                using var db = new AppDbContext();
                var settings = await db.AppSettings.FindAsync(1);
                if (settings is null)
                {
                    settings = new AppSettingsEntity { Id = 1 };
                    db.AppSettings.Add(settings);
                }

                settings.AppEmail = AppEmail;
                settings.AppPasswordEncrypted = SecureStorageService.Encrypt(AppPassword);
                settings.UserEmail = UserEmail;
                settings.IsConfigured = true;
                settings.IsDemoMode = false;
                await db.SaveChangesAsync();

                try
                {
                    await _emailService.SendTestEmailAsync(AppEmail, AppPassword, UserEmail);
                }
                catch
                {
                    // Připojení už bylo ověřeno. Dočasný výpadek při odeslání
                    // testovací zprávy proto neblokuje dokončení konfigurace.
                }

                SetupCompleted?.Invoke();
            }
            finally
            {
                IsSaving = false;
                FinishCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand]
        private async Task ContinueInDemoModeAsync()
        {
            using var db = new AppDbContext();
            var settings = await db.AppSettings.FindAsync(1);
            if (settings is null)
            {
                settings = new AppSettingsEntity { Id = 1 };
                db.AppSettings.Add(settings);
            }

            settings.AppEmail = string.Empty;
            settings.AppPasswordEncrypted = string.Empty;
            settings.UserEmail = string.Empty;
            settings.IsConfigured = true;
            settings.IsDemoMode = true;
            await db.SaveChangesAsync();

            SetupCompleted?.Invoke();
        }
    }
}
