using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;

namespace MovieTracker.Features.Comments;

public record DownVoteCommentCommand(int CommentId, string AppUserId) : IRequest<Response<int>>;

public class DownVoteCommentCommandHandler : IRequestHandler<DownVoteCommentCommand, Response<int>>
{
    private readonly ICommentsRepository _commentsRepository;

    public DownVoteCommentCommandHandler(ICommentsRepository commentsRepository)
    {
        _commentsRepository = commentsRepository;
    }

    public async Task<Response<int>> Handle(DownVoteCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _commentsRepository.DownVoteComment(request.CommentId, request.AppUserId);
            var newRating = await _commentsRepository.GetRatingForComment(request.CommentId);
            return new Response<int>(true, "Successfully downvoted the comment.", newRating);
        }
        catch (Exception ex)
        {
            return new Response<int>(false, ex.Message);
        }
    }
}
