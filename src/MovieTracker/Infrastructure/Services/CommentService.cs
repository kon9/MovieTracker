using AutoMapper;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Infrastructure.Services;

public class CommentService : ICommentService
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly IMapper _mapper;

    public CommentService(ICommentsRepository commentsRepository, IMapper mapper)
    {
        _commentsRepository = commentsRepository;
        _mapper = mapper;
    }

    public async Task<CommentVm> AddCommentAsync(int movieId, CommentDto commentDto, string userId)
    {
        var comment = new Comment
        {
            Content = commentDto.Content,
            MovieId = movieId,
            AppUserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _commentsRepository.CreateAsync(comment);
        return _mapper.Map<CommentVm>(comment);
    }

    public async Task<IEnumerable<CommentVm>> GetCommentsForMovieAsync(int movieId)
    {
        var comments = await _commentsRepository.GetAllCommentsForMovieAsync(movieId);
        return _mapper.Map<IEnumerable<CommentVm>>(comments);
    }

    public async Task<CommentVm> GetCommentAsync(int commentId)
    {
        var comment = await _commentsRepository.GetByIdAsync(commentId);
        return comment == null ? null : _mapper.Map<CommentVm>(comment);
    }

    public async Task<CommentVm> ReplyToCommentAsync(int parentCommentId, CommentDto replyDto, string userId)
    {
        var parentComment = await _commentsRepository.GetByIdAsync(parentCommentId);
        if (parentComment == null) throw new ArgumentException("Parent comment not found.");

        var replyComment = new Comment
        {
            Content = replyDto.Content,
            MovieId = parentComment.MovieId,
            AppUserId = userId,
            CreatedAt = DateTime.UtcNow,
            ParentCommentId = parentCommentId
        };

        await _commentsRepository.CreateAsync(replyComment);
        return _mapper.Map<CommentVm>(replyComment);
    }

    public async Task<Response<int>> UpVoteCommentAsync(int commentId, string userId)
    {
        var comment = await _commentsRepository.GetByIdAsync(commentId);
        if (comment == null)
        {
            throw new ArgumentException("Comment not found.");
        }

        await _commentsRepository.UpVoteComment(commentId, userId);
        var newRating = await _commentsRepository.GetRatingForComment(commentId);
    
        return new Response<int>(true, "Successfully upvoted the comment.", newRating);
    }

    public async Task<Response<int>> DownVoteCommentAsync(int commentId, string userId)
    {
        var comment = await _commentsRepository.GetByIdAsync(commentId);
        if (comment == null)
        {
            throw new ArgumentException("Comment not found.");
        }

        await _commentsRepository.DownVoteComment(commentId, userId);
        var newRating = await _commentsRepository.GetRatingForComment(commentId);

        return new Response<int>(true, "Successfully downvoted the comment.", newRating);
    }
}