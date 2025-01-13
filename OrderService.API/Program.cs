using MassTransit;
using OrderService.API.Services;
using Prometheus;
using MongoDB.Driver;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddMongoDb(builder.Configuration.GetConnectionString("MongoDB"))
    .AddRabbitMQ(rabbitConnectionString: builder.Configuration["RabbitMQ"]);

// Add MongoDB service
builder.Services.AddSingleton<MongoDbService>();

// Add MassTransit
builder.Services.AddMassTransit(configurator =>

       configurator.UsingRabbitMq((context, _configure) =>
       {
           _configure.Host(builder.Configuration["RabbitMQ"]);
       }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Prometheus Middleware
app.UseMetricServer();
app.UseHttpMetrics();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
