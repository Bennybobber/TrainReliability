using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RailWatch.Domain.Integrations;
using RailWatch.Infrastructure.Integrations;
using RailWatch.Infrastructure.Persistence;

namespace RailWatch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRailWatchInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Missing required connection string. Set 'ConnectionStrings__Default' environment variable " +
                "or configure 'ConnectionStrings:Default' in configuration.");
        }

        services.AddDbContext<RailWatchDbContext>(options =>
            options.UseSqlServer(connectionString));

        // External integration registrations (mock now, Darwin later)
        services.AddScoped<IDepartureBoardProvider, MockDepartureBoardProvider>();

        return services;
    }
}