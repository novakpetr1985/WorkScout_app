using CommunityToolkit.Mvvm.ComponentModel;
using WorkScout.Models;

namespace WorkScout.ViewModels
{
    /// <summary>
    /// FEATURE: PORTAL FILTER — propojuje jednu checkbox volbu s uložením preference do DB.
    /// </summary>
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
