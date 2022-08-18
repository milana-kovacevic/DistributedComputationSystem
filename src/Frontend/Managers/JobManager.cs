using Frontend.Data;
using Frontend.Extensions;
using Frontend.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Packaging;
using System.Reflection;

namespace Frontend.Managers
{
    /// <summary>
    /// Class that manages jobs submitted for distributed computation.
    /// </summary>
    public class JobManager : IJobManager
    {
        private static Dictionary<int, Job> activeJobs = new Dictionary<int, Job>();

        public static void Initialize(Dictionary<int, Job> jobs)
        {
            activeJobs.AddRange(jobs);
        }

        public void AddNewJobToQueue(Job job)
        {
            activeJobs.Add(job.Id, job);
        }

        public void CancelJob(int id)
        {
            // TODO
        }
    }
}
