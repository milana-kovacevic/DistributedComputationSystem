using Frontend.Models;

namespace Frontend.Managers
{
    public interface IJobManager
    {
        void AddNewJobToQueue(Job job);

        void CancelJob(int id);
    }
}
