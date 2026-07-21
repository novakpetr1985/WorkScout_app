namespace JobSearchApp.Models
{
    // Jeden jediný řádek v DB - aplikace je jen pro jednoho uživatele,
    // takže žádná tabulka Users, jen jednoduché nastavení.
    // Jmenuje se "AppSettingsEntity" (ne "AppSettings"), aby nekolidovalo
    // s názvem DbSet<AppSettingsEntity> AppSettings v AppDbContext.
    public class AppSettingsEntity
    {
        public int Id { get; set; }

        public string AppEmail { get; set; } = string.Empty;

        // App password se ukládá zašifrované (DPAPI), nikdy v čitelné podobě.
        public string AppPasswordEncrypted { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;

        public bool IsConfigured { get; set; }

        public bool IsDemoMode { get; set; }
    }
}
