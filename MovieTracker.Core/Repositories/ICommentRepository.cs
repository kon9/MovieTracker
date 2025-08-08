using MovieTracker.Core.Entities;

namespace MovieTracker.Core.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(int id);
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<IEnumerable<Comment>> GetByMovieIdAsync(int movieId);
        Task<IEnumerable<Comment>> GetByUserIdAsync(int userId);
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment> UpdateAsync(Comment comment);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsOwnerAsync(int commentId, int userId);
    }
} 