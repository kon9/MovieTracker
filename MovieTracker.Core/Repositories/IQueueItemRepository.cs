using MovieTracker.Core.Entities;

namespace MovieTracker.Core.Repositories
{
    public interface IQueueItemRepository
    {
        Task<QueueItem?> GetByIdAsync(int id);
        Task<IEnumerable<QueueItem>> GetByQueueIdAsync(int queueId);
        Task<QueueItem> CreateAsync(QueueItem queueItem);
        Task<QueueItem> UpdateAsync(QueueItem queueItem);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetNextPositionAsync(int queueId);
    }
} 