using Dapper;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Infrastructure.Data;
using System.Data;

namespace MovieTracker.Infrastructure.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public QueueRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Queue?> GetByIdAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Queue>(
                "SELECT * FROM queues WHERE id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Queue>> GetAllAsync()
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<Queue>("SELECT * FROM queues ORDER BY created_at");
        }

        public async Task<IEnumerable<Queue>> GetByOwnerIdAsync(int ownerId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<Queue>(
                "SELECT * FROM queues WHERE owner_id = @OwnerId ORDER BY created_at", 
                new { OwnerId = ownerId });
        }

        public async Task<IEnumerable<Queue>> GetPublicQueuesAsync()
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<Queue>(
                "SELECT * FROM queues WHERE is_public = true ORDER BY created_at");
        }

        public async Task<IEnumerable<Queue>> GetByMemberIdAsync(int memberId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                SELECT q.* FROM queues q
                INNER JOIN queue_members qm ON q.id = qm.queue_id
                WHERE qm.user_id = @MemberId
                ORDER BY q.created_at";
            
            return await connection.QueryAsync<Queue>(sql, new { MemberId = memberId });
        }

        public async Task<Queue> CreateAsync(Queue queue)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                INSERT INTO queues (name, description, is_public, owner_id, created_at)
                VALUES (@Name, @Description, @IsPublic, @OwnerId, @CreatedAt)
                RETURNING *";
            
            return await connection.QueryFirstAsync<Queue>(sql, queue);
        }

        public async Task<Queue> UpdateAsync(Queue queue)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                UPDATE queues 
                SET name = @Name, description = @Description, is_public = @IsPublic, 
                    updated_at = @UpdatedAt
                WHERE id = @Id
                RETURNING *";
            
            return await connection.QueryFirstAsync<Queue>(sql, queue);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            await connection.ExecuteAsync("DELETE FROM queues WHERE id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM queues WHERE id = @Id", new { Id = id });
            return count > 0;
        }

        public async Task<bool> IsMemberAsync(int queueId, int userId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM queue_members WHERE queue_id = @QueueId AND user_id = @UserId", 
                new { QueueId = queueId, UserId = userId });
            return count > 0;
        }

        public async Task<bool> IsOwnerAsync(int queueId, int userId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM queues WHERE id = @QueueId AND owner_id = @UserId", 
                new { QueueId = queueId, UserId = userId });
            return count > 0;
        }
    }
} 