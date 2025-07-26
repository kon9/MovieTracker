using AutoMapper;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Infrastructure.Interfaces.Repository;
using MovieTracker.Infrastructure.Interfaces.Service;
using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Infrastructure.Services;

public class QueueService : IQueueService
{
    
    private readonly IQueueRepository _queueRepository;
    private readonly IMapper _mapper;

    public QueueService(IMapper mapper, IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
        _mapper = mapper;
    }

    public async Task<QueueVm> GetQueueAsync(int queueId)
    {
        var queue = await _queueRepository.GetByIdAsync(queueId);
        return queue == null ? null : _mapper.Map<QueueVm>(queue);
    }

    public async Task<QueueVm> CreateQueueAsync(QueueDto queueDto)
    {
        var queue = _mapper.Map<Queue>(queueDto);
        await _queueRepository.CreateAsync(queue);
        return _mapper.Map<QueueVm>(queue);
    }

    public Task AddUserToQueueAsync(int queueId, int userId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveUserFromQueueAsync(int queueId, int userId)
    {
        throw new NotImplementedException();
    }

    public Task MoveQueueForwardAsync(int queueId)
    {
        throw new NotImplementedException();
    }

    public Task SwapUsersInQueueAsync(int queueId, int firstUserId, int secondUserId)
    {
        throw new NotImplementedException();
    }
}