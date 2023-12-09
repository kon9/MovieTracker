using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;

namespace MovieTracker.Features.Comments;

public record DownVoteCommentCommand(int CommentId, string AppUserId) : IRequest<Response<int>>;

public class DownVoteCommentHandler : IRequestHandler<DownVoteCommentCommand, Response<int>>
{
    private readonly ICommentService _commentService;

    public DownVoteCommentHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    public async Task<Response<int>> Handle(DownVoteCommentCommand request, CancellationToken cancellationToken)
    {
        return await _commentService.DownVoteCommentAsync(request.CommentId, request.AppUserId);
    }
}
