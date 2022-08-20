using Frontend.Models;

namespace Frontend.Managers
{
    public interface IJobManager
    {
        bool TryAddJob(Job job);

        Task CancelJobAsync(int id);
    }
}
