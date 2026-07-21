namespace JobSearchApp.Models
{
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
