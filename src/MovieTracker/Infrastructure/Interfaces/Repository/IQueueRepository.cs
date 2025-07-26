using System.Collections;

namespace MovieTracker.Infrastructure.Interfaces;

public interface IQueueRepository : IRepository<Queue>
{
    Task<int> AddUserToQueue(int userId, int queueId);
    Task RemoveUserFromQueue(int userId, int queueId);
    Task MoveQueueForward();
    Task SwapUsersInQueue(int queueId, int firstUserId, int secondUserId);
}