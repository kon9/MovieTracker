using MovieTracker.Core.Entities;

namespace MovieTracker.Core.Repositories
{
    public interface IMovieRepository
    {
        Task<Movie?> GetByIdAsync(int id);
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<IEnumerable<Movie>> SearchAsync(string searchTerm);
        Task<IEnumerable<Movie>> GetByGenreAsync(string genre);
        Task<IEnumerable<Movie>> GetByYearAsync(int year);
        Task<Movie> CreateAsync(Movie movie);
        Task<Movie> UpdateAsync(Movie movie);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByTitleAsync(string title);
    }
} 