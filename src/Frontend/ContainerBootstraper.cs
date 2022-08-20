using Frontend.Configuration;
using Frontend.DistributedOrchestrator;
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
            services.AddSingleton<IOrchestrator, DistributedOrchestrator.DistributedOrchestrator>();
            services.AddSingleton<IJobManager, JobManager>();
            services.AddSingleton<JobQueue>();
            services.AddSingleton<IToplogyManager, TopologyManager>();
            services.AddSingleton<IScheduler, DistributedScheduler>();

            // Add services to the container.

            // Add background task for job scheduler.
            services.AddHostedService<SchedulerBackgroundService>();

            // Add controllers for MVC.
            services.AddControllers().AddJsonOptions(x =>
            {
                // serialize enums as strings in api responses (e.g. JobState)
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
