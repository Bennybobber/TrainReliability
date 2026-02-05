using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RailWatch.Infrastructure.Persistence;

public sealed class RailWatchDbContextFactory : IDesignTimeDbContextFactory<RailWatchDbContext>
{
    public RailWatchDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory());

        
        var config = new ConfigurationBuilder().SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("Default");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Missing required connection string. Configure 'ConnectionStrings:Default' in appsettings or set 'ConnectionStrings__Default'.");
            
        }

        return new RailWatchDbContext(
            new DbContextOptionsBuilder<RailWatchDbContext>().UseSqlServer(connectionString).Options
        );
    }
}