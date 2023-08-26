using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;

namespace MovieTracker.Features.Comments;

public record AddCommentCommand(int MovieId, string Content, string AppUserId) : IRequest<Comment>;

public class AddCommentHandler : IRequestHandler<AddCommentCommand, Comment>
{
    private readonly ICommentsRepository _commentsRepository;

    public AddCommentHandler(ICommentsRepository commentsRepository)
    {
        _commentsRepository = commentsRepository;
    }

    public async Task<Comment> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Comment
        {
            Content = request.Content,
            MovieId = request.MovieId,
            AppUserId = request.AppUserId,
            CreatedAt = DateTime.UtcNow
        };

        await _commentsRepository.CreateAsync(comment);

        return comment;
    }
}