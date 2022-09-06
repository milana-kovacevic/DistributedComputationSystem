using Frontend.Models;
using System.Collections.Concurrent;

namespace Frontend.Engine
{
    public interface IAtomicJobScheduler
    {
        Task ScheduleAsync(AtomicJob atomicJob);
    }
}
