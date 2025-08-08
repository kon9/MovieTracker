using MovieTracker.Core.DTOs;

namespace MovieTracker.Core.Services
{
    public interface IMovieService
    {
        Task<MovieDto?> GetByIdAsync(int id);
        Task<IEnumerable<MovieDto>> GetAllAsync();
        Task<IEnumerable<MovieDto>> SearchAsync(string searchTerm);
        Task<IEnumerable<MovieDto>> GetByGenreAsync(string genre);
        Task<IEnumerable<MovieDto>> GetByYearAsync(int year);
        Task<MovieDto> CreateAsync(CreateMovieDto createMovieDto);
        Task<MovieDto> UpdateAsync(int id, UpdateMovieDto updateMovieDto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 