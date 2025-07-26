using Queue = MovieTracker.Models.Queue;

namespace MovieTracker.Infrastructure.Interfaces.Repository;

public interface IQueueRepository : IRepository<Queue>
{
    Task<int> AddUserToQueueAsync(int userId, int queueId);
    Task RemoveUserFromQueueAsync(int userId, int queueId);
    Task MoveQueueForwardAsync();
    Task SwapUsersInQueueAsync(int queueId, int firstUserId, int secondUserId);
}