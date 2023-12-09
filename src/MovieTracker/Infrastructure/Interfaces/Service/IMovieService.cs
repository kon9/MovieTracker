using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Infrastructure.Interfaces;

public interface IMovieService
{
    Task<MovieVm> GetMovieAsync(int id);
    Task<IEnumerable<MovieVm>> GetAllMoviesAsync();
    Task<MovieVm> CreateMovieAsync(MovieDto movieDto);
    Task UpdateMovieAsync(int id, MovieDto movieDto);
    Task DeleteMovieAsync(int id);
    Task<RatingVm> RateMovieAsync(int movieId, RatingDto ratingDto, string userId);
}