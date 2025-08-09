using MovieTracker.Infrastructure.Data;

namespace MovieTracker.Infrastructure.Migrations
{
    public interface IMigrationRunner
    {
        Task RunMigrationsAsync();
        Task<bool> DatabaseExistsAsync();
        Task CreateDatabaseAsync();
    }
}

