using AutoMapper;
using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Features.Comments;

public record GetCommentQuery(int Id) : IRequest<CommentVm>;

public class GetCommentHandler : IRequestHandler<GetCommentQuery, CommentVm>
{
    private readonly ICommentService _commentService;

    public GetCommentHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<CommentVm> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        return await _commentService.GetCommentAsync(request.Id);
    }
}