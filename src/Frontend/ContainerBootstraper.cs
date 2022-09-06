using Frontend.ComputeNodeSwaggerClient;
using Frontend.Configuration;
using Frontend.Data;
using Frontend.Engine;
using Frontend.Managers;
using Frontend.Providers;
using Frontend.Topology;
using System.Text.Json.Serialization;

namespace Frontend
{
    public static class ContainerBootstraper
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFrontendConfiguration, FrontendConfiguration>();
            services.AddSingleton<IConnectionStringProvider, AzureSqlDbConnectionStringProvider>();
            services.AddSingleton<IJobManager, JobManager>();
            services.AddSingleton<JobQueue>();
            services.AddSingleton<IAddressManager, AddressManager>();
            services.AddSingleton<IComputeNodeClientWrapper, ComputeNodeClientWrapper>();
            services.AddSingleton<IScheduler, DistributedScheduler>();
            services.AddSingleton<IAtomicJobScheduler, AtomicJobScheduler>();
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
