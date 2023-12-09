using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Infrastructure.Interfaces;

public interface ICommentService
{
    Task<CommentVm> AddCommentAsync(int movieId, CommentDto commentDto, string userId);
    Task<IEnumerable<CommentVm>> GetCommentsForMovieAsync(int movieId);
    Task<CommentVm> GetCommentAsync(int commentId);
    Task<CommentVm> ReplyToCommentAsync(int parentCommentId, CommentDto replyDto, string userId);
    Task<Response<int>> UpVoteCommentAsync(int commentId, string userId);
    Task<Response<int>> DownVoteCommentAsync(int commentId, string userId);
}