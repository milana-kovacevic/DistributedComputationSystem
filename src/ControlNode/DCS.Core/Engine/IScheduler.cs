using ControlNode.Abstraction.Models;

namespace ControlNode.DCS.Core.Engine
{
    public interface IScheduler
    {
        IEnumerable<Job> GetInProgressTasks();

        Task ScheduleJobAsync(Job job);
    }
}
