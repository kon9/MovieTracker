using System.Data;

namespace MovieTracker.Infrastructure.Data
{
    public interface IDatabaseConnection
    {
        IDbConnection CreateConnection();
        Task<IDbConnection> CreateConnectionAsync();
    }
} 