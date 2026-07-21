using CommunityToolkit.Mvvm.ComponentModel;
using JobSearchApp.Models;

namespace JobSearchApp.ViewModels
{
    public partial class PortalOptionViewModel : ObservableObject
    {
        public int Id { get; }
        public string Name { get; }

        [ObservableProperty]
        private bool isSelected;

        public PortalOptionViewModel(Portal portal)
        {
            Id = portal.Id;
            Name = portal.Name;
            isSelected = portal.IsSelected;
        }

        public event Action<int, bool>? SelectionChanged;

        partial void OnIsSelectedChanged(bool value) => SelectionChanged?.Invoke(Id, value);
    }
}
