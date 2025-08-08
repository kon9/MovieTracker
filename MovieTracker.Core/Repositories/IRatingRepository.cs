using MovieTracker.Core.Entities;

namespace MovieTracker.Core.Repositories
{
    public interface IRatingRepository
    {
        Task<Rating?> GetByIdAsync(int id);
        Task<IEnumerable<Rating>> GetAllAsync();
        Task<Rating?> GetByUserAndMovieAsync(int userId, int movieId);
        Task<IEnumerable<Rating>> GetByMovieIdAsync(int movieId);
        Task<IEnumerable<Rating>> GetByUserIdAsync(int userId);
        Task<double> GetAverageRatingAsync(int movieId);
        Task<int> GetTotalRatingsAsync(int movieId);
        Task<Rating> CreateAsync(Rating rating);
        Task<Rating> UpdateAsync(Rating rating);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByUserAndMovieAsync(int userId, int movieId);
    }
} 