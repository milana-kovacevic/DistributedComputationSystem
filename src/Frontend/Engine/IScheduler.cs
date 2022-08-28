using Frontend.Models;

namespace Frontend.Engine
{
    public interface IScheduler
    {
        IEnumerable<Job> GetInProgressTasks();

        Task ScheduleJobAsync(Job job);
    }
}
