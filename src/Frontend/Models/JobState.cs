namespace Frontend.Models
{
    public enum JobState
    {
        Pending = 0,
        Queued,
        InProgress,
        Succeeded,
        Failed,
        PendingCancellation,
        Cancelled
    }
}
