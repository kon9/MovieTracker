using System.Collections;
using System.Linq.Expressions;
using MovieTracker.Infrastructure.Interfaces.Repository;

namespace MovieTracker.Infrastructure.Repositories;

public class QueueRepository : IQueueRepository
{
    public Task<IEnumerable<Models.Queue>> GetAllAsync(Expression<Func<Models.Queue, bool>> predicate = null)
    {
        throw new NotImplementedException();
    }

    public Task<Models.Queue> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(Models.Queue entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Models.Queue entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Models.Queue entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> AddUserToQueueAsync(int userId, int queueId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveUserFromQueueAsync(int userId, int queueId)
    {
        throw new NotImplementedException();
    }

    public Task MoveQueueForwardAsync()
    {
        throw new NotImplementedException();
    }

    public Task SwapUsersInQueueAsync(int queueId, int firstUserId, int secondUserId)
    {
        throw new NotImplementedException();
    }
}