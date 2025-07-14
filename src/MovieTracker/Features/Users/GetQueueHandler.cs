using MediatR;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Features.Users;

public record GetQueueQuery(QueueVm queue) : IRequest;
public class GetQueueHandler
{
    
}