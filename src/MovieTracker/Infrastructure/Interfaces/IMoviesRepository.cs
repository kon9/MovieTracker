using MovieTracker.Models;

namespace MovieTracker.Infrastructure.Interfaces;

public interface IMoviesRepository : IRepository<Movie>
{
    Task<IEnumerable<Rating>> GetAllRatingsForMovie(int movieId);
    Task<double> GetAverageRatingForMovie(int movieId);
    Task AddRatingToMovie(int movieId, Rating rating);
    Task UpdateRatingForMovie(int movieId, Rating rating);
}