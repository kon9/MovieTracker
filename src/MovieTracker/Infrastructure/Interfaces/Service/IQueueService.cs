using MovieTracker.Models.ViewModels;

namespace MovieTracker.Infrastructure.Interfaces;

public interface IQueueService
{
    Task<QueueVm> GetQueue(int queueId);
    Task CreateQueue(QueueVm queueVm);
    Task AddUserToQueue(int queueId, int userId);
    Task RemoveUserFromQueue(int queueId, int userId);
    Task MoveQueueForward(int queueId);
    Task SwapUsersInQueue(int queueId, int firstUserId, int secondUserId);
}