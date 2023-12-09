using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Features.Comments;

public record AddCommentCommand(int MovieId, string Content, string AppUserId) : IRequest<CommentVm>;

public class AddCommentHandler : IRequestHandler<AddCommentCommand, CommentVm>
{
    private readonly ICommentService _commentService;

    public AddCommentHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<CommentVm> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        return await _commentService.AddCommentAsync(request.MovieId, new CommentDto { Content = request.Content }, request.AppUserId);
    }
}