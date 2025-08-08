using Dapper;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Infrastructure.Data;
using System.Data;

namespace MovieTracker.Infrastructure.Repositories
{
    public class QueueItemRepository : IQueueItemRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public QueueItemRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<QueueItem?> GetByIdAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<QueueItem>(
                "SELECT * FROM queue_items WHERE id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<QueueItem>> GetByQueueIdAsync(int queueId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            return await connection.QueryAsync<QueueItem>(
                "SELECT * FROM queue_items WHERE queue_id = @QueueId ORDER BY position", 
                new { QueueId = queueId });
        }

        public async Task<QueueItem> CreateAsync(QueueItem queueItem)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                INSERT INTO queue_items (queue_id, title, description, type, external_id, image_url, 
                                       position, status, added_at, added_by_id)
                VALUES (@QueueId, @Title, @Description, @Type, @ExternalId, @ImageUrl, 
                       @Position, @Status, @AddedAt, @AddedById)
                RETURNING *";
            
            return await connection.QueryFirstAsync<QueueItem>(sql, queueItem);
        }

        public async Task<QueueItem> UpdateAsync(QueueItem queueItem)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var sql = @"
                UPDATE queue_items 
                SET title = @Title, description = @Description, type = @Type, 
                    external_id = @ExternalId, image_url = @ImageUrl, position = @Position, 
                    status = @Status, completed_at = @CompletedAt
                WHERE id = @Id
                RETURNING *";
            
            return await connection.QueryFirstAsync<QueueItem>(sql, queueItem);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            await connection.ExecuteAsync("DELETE FROM queue_items WHERE id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM queue_items WHERE id = @Id", new { Id = id });
            return count > 0;
        }

        public async Task<int> GetNextPositionAsync(int queueId)
        {
            using var connection = await _dbConnection.CreateConnectionAsync();
            var maxPosition = await connection.ExecuteScalarAsync<int?>(
                "SELECT MAX(position) FROM queue_items WHERE queue_id = @QueueId", 
                new { QueueId = queueId });
            
            return (maxPosition ?? 0) + 1;
        }
    }
} 