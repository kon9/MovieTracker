using MovieTracker.Core.DTOs;

namespace MovieTracker.Core.Services
{
    public interface ICommentService
    {
        Task<CommentDto?> GetByIdAsync(int id);
        Task<IEnumerable<CommentDto>> GetAllAsync();
        Task<IEnumerable<CommentDto>> GetByMovieIdAsync(int movieId);
        Task<IEnumerable<CommentDto>> GetByUserIdAsync(int userId);
        Task<CommentDto> CreateAsync(int userId, CreateCommentDto createCommentDto);
        Task<CommentDto> UpdateAsync(int id, int userId, UpdateCommentDto updateCommentDto);
        Task DeleteAsync(int id, int userId);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsOwnerAsync(int commentId, int userId);
    }
} 