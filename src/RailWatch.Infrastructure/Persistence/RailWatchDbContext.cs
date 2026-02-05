using Microsoft.EntityFrameworkCore;

namespace RailWatch.Infrastructure.Persistence;

public sealed class RailWatchDbContext(DbContextOptions<RailWatchDbContext> options) : DbContext(options)
{

}