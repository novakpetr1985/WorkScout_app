namespace WorkScout.Models
{
    /// <summary>
    /// EXTENSION POINT: INBOUND MAIL — metadata zprávy načtené z aplikační schránky.
    /// ExternalMessageId slouží k idempotentnímu importu a ochraně před duplicitami.
    /// </summary>
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
