using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Infrastructure.Services;

public class QueueService : IQueueService
{
    public Task<QueueVm> GetQueue(int queueId)
    {
        throw new NotImplementedException();
    }

    public Task CreateQueue(QueueVm queueVm)
    {
        throw new NotImplementedException();
    }

    public Task AddUserToQueue(int queueId, int userId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveUserFromQueue(int queueId, int userId)
    {
        throw new NotImplementedException();
    }

    public Task MoveQueueForward(int queueId)
    {
        throw new NotImplementedException();
    }

    public Task SwapUsersInQueue(int queueId, int firstUserId, int secondUserId)
    {
        throw new NotImplementedException();
    }
}