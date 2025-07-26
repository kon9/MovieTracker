using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Infrastructure.Interfaces.Service;

public interface IQueueService
{
    Task<QueueVm> GetQueueAsync(int queueId);
    Task<QueueVm> CreateQueueAsync(QueueDto queueDto);
    Task AddUserToQueueAsync(int queueId, int userId);
    Task RemoveUserFromQueueAsync(int queueId, int userId);
    Task MoveQueueForwardAsync(int queueId);
    Task SwapUsersInQueueAsync(int queueId, int firstUserId, int secondUserId);
}