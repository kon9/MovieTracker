using MovieTracker.Core.DTOs;

namespace MovieTracker.Core.Services
{
    public interface IQueueService
    {
        Task<QueueDto?> GetByIdAsync(int id);
        Task<IEnumerable<QueueDto>> GetAllAsync();
        Task<IEnumerable<QueueDto>> GetByOwnerIdAsync(int ownerId);
        Task<IEnumerable<QueueDto>> GetPublicQueuesAsync();
        Task<IEnumerable<QueueDto>> GetByMemberIdAsync(int memberId);
        Task<QueueDto> CreateAsync(int ownerId, CreateQueueDto createQueueDto);
        Task<QueueDto> UpdateAsync(int id, int userId, UpdateQueueDto updateQueueDto);
        Task DeleteAsync(int id, int userId);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsMemberAsync(int queueId, int userId);
        Task<bool> IsOwnerAsync(int queueId, int userId);
        Task<QueueDto> AddItemToQueueAsync(int queueId, int userId, AddItemToQueueDto addItemDto);
        Task<QueueDto> UpdateQueueItemAsync(int queueId, int itemId, int userId, UpdateQueueItemDto updateItemDto);
        Task<QueueDto> RemoveItemFromQueueAsync(int queueId, int itemId, int userId);
        Task<QueueDto> AddMemberToQueueAsync(int queueId, int ownerId, AddMemberToQueueDto addMemberDto);
        Task<QueueDto> RemoveMemberFromQueueAsync(int queueId, int memberId, int userId);
        Task<QueueDto> UpdateMemberRoleAsync(int queueId, int memberId, int userId, string newRole);
    }
} 