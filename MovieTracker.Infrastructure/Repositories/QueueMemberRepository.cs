using Dapper;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Infrastructure.Data;
using System.Data;

namespace MovieTracker.Infrastructure.Repositories
{
    public class QueueMemberRepository : IQueueMemberRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public QueueMemberRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<QueueMember?> GetByIdAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<QueueMember>(
                "SELECT * FROM queue_members WHERE id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<QueueMember>> GetByQueueIdAsync(int queueId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<QueueMember>(
                "SELECT * FROM queue_members WHERE queue_id = @QueueId ORDER BY joined_at", 
                new { QueueId = queueId });
        }

        public async Task<QueueMember?> GetByQueueAndUserIdAsync(int queueId, int userId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<QueueMember>(
                "SELECT * FROM queue_members WHERE queue_id = @QueueId AND user_id = @UserId", 
                new { QueueId = queueId, UserId = userId });
        }

        public async Task<QueueMember> CreateAsync(QueueMember queueMember)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                INSERT INTO queue_members (queue_id, user_id, role, joined_at)
                VALUES (@QueueId, @UserId, @Role, @JoinedAt)
                RETURNING *";
            
            return await connection.QueryFirstAsync<QueueMember>(sql, queueMember);
        }

        public async Task<QueueMember> UpdateAsync(QueueMember queueMember)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                UPDATE queue_members 
                SET role = @Role
                WHERE id = @Id
                RETURNING *";
            
            return await connection.QueryFirstAsync<QueueMember>(sql, queueMember);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            await connection.ExecuteAsync("DELETE FROM queue_members WHERE id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM queue_members WHERE id = @Id", new { Id = id });
            return count > 0;
        }

        public async Task<bool> ExistsByQueueAndUserAsync(int queueId, int userId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM queue_members WHERE queue_id = @QueueId AND user_id = @UserId", 
                new { QueueId = queueId, UserId = userId });
            return count > 0;
        }
    }
} 