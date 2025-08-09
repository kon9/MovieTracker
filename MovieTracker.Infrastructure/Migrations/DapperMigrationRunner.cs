using System.Data;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MovieTracker.Infrastructure.Data;
using Npgsql;

namespace MovieTracker.Infrastructure.Migrations
{
    public class DapperMigrationRunner : IMigrationRunner
    {
        private readonly IDatabaseConnection _databaseConnection;
        private readonly string _connectionString;
        private readonly ILogger<DapperMigrationRunner> _logger;

        public DapperMigrationRunner(IDatabaseConnection databaseConnection, IConfiguration configuration, ILogger<DapperMigrationRunner> logger)
        {
            _databaseConnection = databaseConnection;
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("DefaultConnection");
            _logger = logger;
        }

        public async Task<bool> DatabaseExistsAsync()
        {
            try
            {
                var masterConnectionString = GetMasterConnectionString(_connectionString);
                using var connection = new NpgsqlConnection(masterConnectionString);
                await connection.OpenAsync();
                
                var databaseName = GetDatabaseName(_connectionString);
                var sql = "SELECT 1 FROM pg_database WHERE datname = @databaseName";
                var result = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { databaseName });
                
                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if database exists");
                return false;
            }
        }

        public async Task CreateDatabaseAsync()
        {
            try
            {
                var masterConnectionString = GetMasterConnectionString(_connectionString);
                using var connection = new NpgsqlConnection(masterConnectionString);
                await connection.OpenAsync();
                
                var databaseName = GetDatabaseName(_connectionString);
                var sql = $"CREATE DATABASE \"{databaseName}\"";
                await connection.ExecuteAsync(sql);
                
                _logger.LogInformation($"Database '{databaseName}' created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating database");
                throw;
            }
        }

        public async Task RunMigrationsAsync()
        {
            try
            {
                // Ensure database exists
                if (!await DatabaseExistsAsync())
                {
                    await CreateDatabaseAsync();
                }

                using var connection = await _databaseConnection.CreateConnectionAsync();

                // Create migrations table if it doesn't exist
                await EnsureMigrationsTableExistsAsync(connection);

                // Get all migration classes
                var migrations = GetAllMigrations();
                
                // Get applied migrations
                var appliedMigrations = await GetAppliedMigrationsAsync(connection);

                // Apply pending migrations
                foreach (var migration in migrations.OrderBy(m => m.Version))
                {
                    if (!appliedMigrations.Contains(migration.Version))
                    {
                        await ApplyMigrationAsync(connection, migration);
                    }
                }

                _logger.LogInformation("All migrations applied successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running migrations");
                throw;
            }
        }

        private async Task EnsureMigrationsTableExistsAsync(IDbConnection connection)
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS __migrations (
                    version INTEGER PRIMARY KEY,
                    description VARCHAR(255) NOT NULL,
                    applied_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                )";

            await connection.ExecuteAsync(sql);
        }

        private List<Migration> GetAllMigrations()
        {
            var migrationType = typeof(Migration);
            var migrations = Assembly.GetAssembly(migrationType)?
                .GetTypes()
                .Where(t => t.IsSubclassOf(migrationType) && !t.IsAbstract)
                .Select(t => (Migration)Activator.CreateInstance(t)!)
                .ToList() ?? new List<Migration>();

            return migrations;
        }

        private async Task<HashSet<int>> GetAppliedMigrationsAsync(IDbConnection connection)
        {
            var sql = "SELECT version FROM __migrations";
            var versions = await connection.QueryAsync<int>(sql);
            return new HashSet<int>(versions);
        }

        private async Task ApplyMigrationAsync(IDbConnection connection, Migration migration)
        {
            _logger.LogInformation($"Applying migration {migration.Version}: {migration.Description}");

            using var transaction = connection.BeginTransaction();
            try
            {
                // Execute the migration script
                await connection.ExecuteAsync(migration.UpScript, transaction: transaction);

                // Record the migration
                var sql = "INSERT INTO __migrations (version, description) VALUES (@version, @description)";
                await connection.ExecuteAsync(sql, new { version = migration.Version, description = migration.Description }, transaction: transaction);

                transaction.Commit();
                _logger.LogInformation($"Migration {migration.Version} applied successfully");
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        private static string GetMasterConnectionString(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString)
            {
                Database = "postgres" // Connect to default postgres database
            };
            return builder.ToString();
        }

        private static string GetDatabaseName(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            return builder.Database;
        }
    }
}
