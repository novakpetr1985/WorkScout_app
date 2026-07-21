namespace JobSearchApp.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int JobListingId { get; set; }
        public JobListing JobListing { get; set; } = null!;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? SubmittedAtUtc { get; set; }
        public string CvPath { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public ICollection<InboundMessage> InboundMessages { get; set; } = new List<InboundMessage>();
    }
}
