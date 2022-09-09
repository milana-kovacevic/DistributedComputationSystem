namespace ControlNode.Abstraction.Models
{
    public enum AtomicJobState
    {
        NotRan = 0,
        InProgress,
        Succeeded,
        Failed,
        Cancelled
    }
}
