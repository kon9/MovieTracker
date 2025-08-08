using MovieTracker.Core.Entities;

namespace MovieTracker.Core.Repositories
{
    public interface IQueueRepository
    {
        Task<Queue?> GetByIdAsync(int id);
        Task<IEnumerable<Queue>> GetAllAsync();
        Task<IEnumerable<Queue>> GetByOwnerIdAsync(int ownerId);
        Task<IEnumerable<Queue>> GetPublicQueuesAsync();
        Task<IEnumerable<Queue>> GetByMemberIdAsync(int memberId);
        Task<Queue> CreateAsync(Queue queue);
        Task<Queue> UpdateAsync(Queue queue);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsMemberAsync(int queueId, int userId);
        Task<bool> IsOwnerAsync(int queueId, int userId);
    }
} 