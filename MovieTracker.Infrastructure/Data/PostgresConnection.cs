using System.Data;
using Npgsql;

namespace MovieTracker.Infrastructure.Data
{
    public class PostgresConnection : IDatabaseConnection
    {
        private readonly string _connectionString;

        public PostgresConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
} 