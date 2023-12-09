using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Features.Comments;

public record ReplyToCommentCommand(int CommentId, string Content, string AppUserId) : IRequest<CommentVm>;

public class ReplyToCommentHandler : IRequestHandler<ReplyToCommentCommand, CommentVm>
{
    private readonly ICommentService _commentService;

    public ReplyToCommentHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<CommentVm> Handle(ReplyToCommentCommand request, CancellationToken cancellationToken)
    {
        return await _commentService.ReplyToCommentAsync(request.CommentId, new CommentDto { Content = request.Content }, request.AppUserId);
    }
}

