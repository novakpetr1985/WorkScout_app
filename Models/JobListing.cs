namespace WorkScout.Models
{
    /// <summary>
    /// EXTENSION POINT: JOB INGESTION — normalizovaný záznam společný pro všechny portály.
    /// URL je v databázi unikátní a tvoří první úroveň deduplikace.
    /// </summary>
    public class JobListing
    {
        public int Id { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Profession { get; set; } = string.Empty;
        public string Portal { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime DateFound { get; set; }
    }
}
