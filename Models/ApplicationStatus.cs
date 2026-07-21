namespace WorkScout.Models
{
    /// <summary>EXTENSION POINT: APPLICATION FLOW — podporované stavy životního cyklu žádosti.</summary>
    public enum ApplicationStatus
    {
        Draft,
        AwaitingApproval,
        Sent,
        ResponseReceived,
        Interview,
        Rejected,
        Offer,
        Withdrawn
    }
}
