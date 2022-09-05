using Frontend.Models;

namespace Frontend.Engine
{
    public interface IAtomicJobScheduler
    {
        Task<AtomicJobResult> ScheduleAsync(AtomicJob atomicJob);
    }
}
