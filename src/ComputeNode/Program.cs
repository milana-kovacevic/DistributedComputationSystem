using ComputeNode.Executor;
using ComputeNode.Models;
using System.Text.Json.Serialization;
using System.Xml.Schema;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. JobState)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add services to the container.

builder.Services.AddSingleton<IJobExecutor, AtomicJobExecutor>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

