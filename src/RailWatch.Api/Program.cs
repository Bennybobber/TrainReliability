using RailWatch.Infrastructure.Integrations;
using RailWatch.Infrastructure.interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDepartureBoardProvider, MockDepartureBoardProvider>();


var app = builder.Build();

app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

app.Run();
