namespace WorkScout.Models
{
    /// <summary>
    /// FEATURE: SINGLE-USER SETTINGS
    /// Jediný řádek s ID 1 nahrazuje uživatelské účty, protože WorkScout 1.x je
    /// lokální aplikace pro jednu osobu.
    /// </summary>
    public class AppSettingsEntity
    {
        public int Id { get; set; }

        public string AppEmail { get; set; } = string.Empty;

        // SECURITY: Hodnota obsahuje pouze DPAPI ciphertext, nikdy otevřený app password.
        public string AppPasswordEncrypted { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;

        public bool IsConfigured { get; set; }

        public bool IsDemoMode { get; set; }
    }
}
