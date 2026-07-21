namespace JobSearchApp.Models
{
    public class InboundMessage
    {
        public int Id { get; set; }
        public int? JobApplicationId { get; set; }
        public JobApplication? JobApplication { get; set; }
        public string ExternalMessageId { get; set; } = string.Empty;
        public string FromAddress { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string BodyPreview { get; set; } = string.Empty;
        public DateTime ReceivedAtUtc { get; set; }
        public bool IsRead { get; set; }
    }
}
