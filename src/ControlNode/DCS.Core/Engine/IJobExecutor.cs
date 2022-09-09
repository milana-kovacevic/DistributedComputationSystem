using ControlNode.Abstraction.Models;

namespace ControlNode.DCS.Core.Engine
{
    public interface IAtomicJobScheduler
    {
        Task ScheduleAsync(AtomicJob atomicJob);
    }
}
