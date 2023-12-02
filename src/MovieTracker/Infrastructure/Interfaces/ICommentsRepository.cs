using MovieTracker.Models;

namespace MovieTracker.Infrastructure.Interfaces;

public interface ICommentsRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetAllCommentsForMovieAsync(int movieId);
    Task UpdateComment(Comment comment);
    Task UpVoteComment(int commentId, string appUserId);
    Task DownVoteComment(int commentId, string appUserId);
    Task<int> GetRatingForComment(int commentId);
}