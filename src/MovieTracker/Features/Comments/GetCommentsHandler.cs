using AutoMapper;
using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Features.Comments;

public record GetCommentsQuery(int MovieId) : IRequest<IEnumerable<CommentVm>>;

public class GetCommentsHandler : IRequestHandler<GetCommentsQuery, IEnumerable<CommentVm>>
{
    private readonly ICommentService _commentService;

    public GetCommentsHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<IEnumerable<CommentVm>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        return await _commentService.GetCommentsForMovieAsync(request.MovieId);
    }
}