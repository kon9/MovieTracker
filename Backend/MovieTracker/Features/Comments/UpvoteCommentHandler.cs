using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;

namespace MovieTracker.Features.Comments;

public record UpVoteCommentCommand(int CommentId, string AppUserId) : IRequest<Response<int>>;

public class UpVoteCommentCommandHandler : IRequestHandler<UpVoteCommentCommand, Response<int>>
{
    private readonly ICommentsRepository _commentsRepository;

    public UpVoteCommentCommandHandler(ICommentsRepository commentsRepository)
    {
        _commentsRepository = commentsRepository;
    }

    public async Task<Response<int>> Handle(UpVoteCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _commentsRepository.UpVoteComment(request.CommentId, request.AppUserId);
            var newRating = await _commentsRepository.GetRatingForComment(request.CommentId);
            return new Response<int>(true, "Successfully upvoted the comment.", newRating);
        }
        catch (Exception ex)
        {
            return new Response<int>(false, ex.Message);
        }
    }
}
