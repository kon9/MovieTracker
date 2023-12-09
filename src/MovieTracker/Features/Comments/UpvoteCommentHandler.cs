using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;

namespace MovieTracker.Features.Comments;

public record UpVoteCommentCommand(int CommentId, string AppUserId) : IRequest<Response<int>>;

public class UpVoteCommentHandler : IRequestHandler<UpVoteCommentCommand, Response<int>>
{
    private readonly ICommentService _commentService;

    public UpVoteCommentHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<Response<int>> Handle(UpVoteCommentCommand request, CancellationToken cancellationToken)
    {
        return await _commentService.UpVoteCommentAsync(request.CommentId, request.AppUserId);
    }
}
