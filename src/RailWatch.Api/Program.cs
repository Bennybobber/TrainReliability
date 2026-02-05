using RailWatch.Infrastructure;
using RailWatch.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRailWatchInfrastructure(builder.Configuration);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<RailWatchDbContext>();

var canConnect = await db.Database.CanConnectAsync();
if (!canConnect)
{
    throw new InvalidOperationException("Database is configured but cannot be reached. Check connection string and SQL Server availability.");
}

app.Logger.LogInformation("Starting RailWatch API in {Environment} environment...", app.Environment.EnvironmentName);

app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

app.Run();