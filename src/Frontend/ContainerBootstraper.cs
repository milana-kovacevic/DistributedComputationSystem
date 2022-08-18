using Frontend.Configuration;
using System.Text.Json.Serialization;

namespace Frontend
{
    public static class ContainerBootstraper
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFrontendConfiguration, FrontendConfiguration>();

            // Add services to the container.
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
