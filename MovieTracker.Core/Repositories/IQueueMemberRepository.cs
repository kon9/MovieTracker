using MovieTracker.Core.Entities;

namespace MovieTracker.Core.Repositories
{
    public interface IQueueMemberRepository
    {
        Task<QueueMember?> GetByIdAsync(int id);
        Task<IEnumerable<QueueMember>> GetByQueueIdAsync(int queueId);
        Task<QueueMember?> GetByQueueAndUserIdAsync(int queueId, int userId);
        Task<QueueMember> CreateAsync(QueueMember queueMember);
        Task<QueueMember> UpdateAsync(QueueMember queueMember);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByQueueAndUserAsync(int queueId, int userId);
    }
} 