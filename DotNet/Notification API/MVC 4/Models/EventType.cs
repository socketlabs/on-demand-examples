namespace NotificationApi.TestEndpoint.Mvc4.Models
{
    /// <summary>
    /// Enumeration of the Available Event Types
    /// </summary>
    public enum EventType
    {
        Delivered,
        Complaint,
        Failed,
        Validation,
        Tracking,
        Unknown
    }
}