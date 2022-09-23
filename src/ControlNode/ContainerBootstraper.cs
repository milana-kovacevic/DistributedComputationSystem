using ControlNode.Configuration;
using ControlNode.DCS.Core.ComputeNodeSwaggerClient;
using ControlNode.DCS.Core.Engine;
using ControlNode.DCS.Core.Managers;
using ControlNode.DCS.Core.Topology;
using ControlNode.Abstraction.Data;
using ControlNode.Frontend.Providers;
using System.Text.Json.Serialization;

namespace Frontend
{
    public static class ContainerBootstraper
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IControlNodeConfiguration, ControlNodeConfiguration>();
            services.AddSingleton<IConnectionStringProvider, AzureSqlDbConnectionStringProvider>();
            services.AddSingleton<IJobManager, JobManager>();
            services.AddSingleton<JobQueue>();
            services.AddSingleton<IAddressManager, AddressManager>();
            services.AddSingleton<IComputeNodeClientWrapper, ComputeNodeClientWrapper>();
            services.AddSingleton<IScheduler, DistributedScheduler>();
            services.AddSingleton<IAtomicJobScheduler, AtomicJobScheduler>();
            services.AddSingleton<ComputeNodeJobExecutor>();
            services.AddSingleton<JobExecutionMonitor>();
            services.AddSingleton<DbEntityManager>();

            // Add services to the container.

            // Add background task for job scheduler.
            services.AddHostedService<SchedulerBackgroundService>();

            // Add controllers for MVC.
            services.AddControllers().AddJsonOptions(x =>
            {
                // serialize enums as strings in api responses (e.g. JobState)
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // Enable https rederection once HTTPS is working.
            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
