using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health checks and UI configuration
builder.Services.AddHealthChecksUI(
    settings =>
    {
        settings.AddHealthCheckEndpoint("Service Notification", "http://localhost:5135/health");
        settings.AddHealthCheckEndpoint("Service Order", "http://localhost:5130/health");
        settings.SetEvaluationTimeInSeconds(3);
        settings.SetApiMaxActiveRequests(3);
    })
    .AddSqlServerStorage("Server=(Local); DataBase=HealthCheckAppMonDb; User Id=sa; Password=asdfghjk; Encrypt=False");

// Prometheus Middleware
builder.Services.AddHealthChecks();
builder.Services.AddMetrics();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMetricServer();  // Prometheus metrics
app.UseHttpMetrics();   // Http metrics

app.UseHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
    options.AddCustomStylesheet("health-check-ui.css");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Health checks UI
app.UseHealthChecksUI();

app.Run();
