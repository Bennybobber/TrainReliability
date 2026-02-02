using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RailWatch.Domain.Integrations;
using RailWatch.Infrastructure.Integrations;

namespace RailWatch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRailWatchInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDepartureBoardProvider, MockDepartureBoardProvider>();
        return services;
    }
}