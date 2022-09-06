using ComputeNode.Controllers;
using ComputeNode.Executor;
using ComputeNode.Executors;
using Frontend.ComputeNodeSwaggerClient;
using Frontend.Configuration;
using Frontend.Controllers;
using Frontend.Data;
using Frontend.Engine;
using Frontend.Managers;
using Frontend.Providers;
using Frontend.Topology;
using IntegrationTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestCommons
{
    public static  class TestBootstraper
    {
        public static void ConfigureServices_Frontend(IServiceCollection services)
        {
            // Add services to the TEST container.

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

            // Loggers
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<ILogger<DistributedScheduler>, Logger<DistributedScheduler>>();
            services.AddSingleton<ILogger<AtomicJobScheduler>, Logger<AtomicJobScheduler>>();
            services.AddSingleton<ILogger<JobsController>, Logger<JobsController>>();
            services.AddSingleton<ILogger<DbEntityManager>, Logger<DbEntityManager>>();
            services.AddSingleton<ILogger<JobExecutionMonitor>, Logger<JobExecutionMonitor>>();

            // Controllers
            services.AddScoped<JobsController>();

            // Add background task for job scheduler.
            //services.AddHostedService<SchedulerBackgroundService>();

            // Configure database context.
            {
                var connectionStringProvider = new LocalSqlDbConnectionStringProvider();
                services.AddDbContext<JobContext>(options => options.UseSqlServer(connectionStringProvider.GetConnectionString()));
            }

        }

        public static void ConfigureServices_ComputeNode(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<ILogger<AtomicJobExecutor>, Logger<AtomicJobExecutor>>();
            services.AddSingleton<IAtomicJobExecutor, AtomicJobExecutor>();

            services.AddSingleton<ISpecificJobExecutorFactory, SpecificJobExecutorFactory>();
            services.AddSingleton<CalculateNumberOfDigitsExecutor>();

            services.AddSingleton<ILogger<AtomicJobController>, Logger<AtomicJobController>>();
            services.AddScoped<AtomicJobController>();
        }
    }
}
