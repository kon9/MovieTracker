using Dapper;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Infrastructure.Data;
using System.Data;

namespace MovieTracker.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public UserRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM users WHERE id = @Id", new { Id = id });
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM users WHERE username = @Username", new { Username = username });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM users WHERE email = @Email", new { Email = email });
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<User>("SELECT * FROM users ORDER BY created_at");
        }

        public async Task<User> CreateAsync(User user)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                INSERT INTO users (username, email, password_hash, created_at)
                VALUES (@Username, @Email, @PasswordHash, @CreatedAt)
                RETURNING *";
            
            return await connection.QueryFirstAsync<User>(sql, user);
        }

        public async Task<User> UpdateAsync(User user)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                UPDATE users 
                SET username = @Username, email = @Email, password_hash = @PasswordHash, 
                    last_login_at = @LastLoginAt
                WHERE id = @Id
                RETURNING *";
            
            return await connection.QueryFirstAsync<User>(sql, user);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            await connection.ExecuteAsync("DELETE FROM users WHERE id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM users WHERE id = @Id", new { Id = id });
            return count > 0;
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM users WHERE username = @Username", new { Username = username });
            return count > 0;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM users WHERE email = @Email", new { Email = email });
            return count > 0;
        }
    }
} 