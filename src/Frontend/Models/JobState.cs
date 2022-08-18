namespace Frontend.Models
{
    public enum JobState
    {
        Pending = 0,
        InProgress,
        Succeeded,
        Failed,
        PendingCancellation,
        Cancelled
    }
}
