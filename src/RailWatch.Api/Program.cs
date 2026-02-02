using RailWatch.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        $"Missing required connection string. Set 'ConnectionStrings__Default' environment variable " +
        $"or configure 'ConnectionStrings:Default'. Environment: '{builder.Environment.EnvironmentName}'.");

}

builder.Services.AddRailWatchInfrastructure(builder.Configuration);

var app = builder.Build();

app.Logger.LogInformation("Starting RailWatch API in {Environment} environment...", app.Environment.EnvironmentName);

app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

app.Run();
