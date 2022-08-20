using Frontend.Models;

namespace Frontend.Engine
{
    public interface IScheduler
    {
        Task ScheduleJobAsync(Job job);
    }
}
