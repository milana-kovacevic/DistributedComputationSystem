using Frontend;
using Microsoft.EntityFrameworkCore;
using Frontend.Data;
using Frontend.Models;
using Frontend.Providers;

var builder = WebApplication.CreateBuilder(args);

// Configure services.
ContainerBootstraper.ConfigureServices(builder.Services);

// Configure database context.
{
    var connectionStringProvider = new AzureSqlDbConnectionStringProvider(builder.Configuration);
    builder.Services.AddDbContext<JobContext>(options => options.UseSqlServer(connectionStringProvider.GetConnectionString()));
}

// Configure authentication
// TODO AAD Auth
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearerConfiguration(
//    builder.Configuration.Issuer,
//    builder.Configuration.Audience
//    );

var app = builder.Build();

// Setup the database.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
