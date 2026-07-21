namespace WorkScout.Models
{
    /// <summary>FEATURE: PORTAL FILTER — zdroj nabídek a uložená volba uživatele.</summary>
    public class Portal
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSelected { get; set; } = true;
    }
}
