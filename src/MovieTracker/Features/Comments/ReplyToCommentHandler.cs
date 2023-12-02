using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;

namespace MovieTracker.Features.Comments;

public record ReplyToCommentCommand(int CommentId, string Content, string AppUserId) : IRequest<Comment>;

public class ReplyToCommentHandler : IRequestHandler<ReplyToCommentCommand, Comment>
{
    private readonly ICommentsRepository _commentsRepository;

    public ReplyToCommentHandler(ICommentsRepository commentsRepository)
    {
        _commentsRepository = commentsRepository;
    }

    public async Task<Comment> Handle(ReplyToCommentCommand request, CancellationToken cancellationToken)
    {
        var parentComment = await _commentsRepository.GetByIdAsync(request.CommentId);
        if (parentComment == null) throw new ArgumentException("Parent comment not found.");
        var replyComment = new Comment
        {
            Content = request.Content,
            MovieId = parentComment.MovieId,
            AppUserId = request.AppUserId,
            CreatedAt = DateTime.UtcNow,
            ParentCommentId = request.CommentId
        };

        await _commentsRepository.CreateAsync(replyComment);

        return replyComment;
    }
}

