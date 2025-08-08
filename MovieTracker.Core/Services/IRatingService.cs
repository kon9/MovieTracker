using MovieTracker.Core.DTOs;

namespace MovieTracker.Core.Services
{
    public interface IRatingService
    {
        Task<RatingDto?> GetByIdAsync(int id);
        Task<IEnumerable<RatingDto>> GetAllAsync();
        Task<RatingDto?> GetByUserAndMovieAsync(int userId, int movieId);
        Task<IEnumerable<RatingDto>> GetByMovieIdAsync(int movieId);
        Task<IEnumerable<RatingDto>> GetByUserIdAsync(int userId);
        Task<double> GetAverageRatingAsync(int movieId);
        Task<int> GetTotalRatingsAsync(int movieId);
        Task<RatingDto> CreateAsync(int userId, CreateRatingDto createRatingDto);
        Task<RatingDto> UpdateAsync(int userId, int movieId, UpdateRatingDto updateRatingDto);
        Task DeleteAsync(int userId, int movieId);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByUserAndMovieAsync(int userId, int movieId);
    }
} 