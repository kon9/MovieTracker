using Microsoft.EntityFrameworkCore;
using MovieTracker.Infrastructure.Data;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;

namespace MovieTracker.Infrastructure.Repo;
public class MoviesRepository : Repository<Movie>, IMoviesRepository
{
    public MoviesRepository(ApplicationDbContext context) : base(context)
    {

    }


    public async Task<IEnumerable<Rating>> GetAllRatingsForMovie(int movieId)
    {
        return await _context.Ratings
            .Where(rating => rating.MovieId == movieId)
            .ToListAsync();
    }

    public async Task<double> GetAverageRatingForMovie(int movieId)
    {
        return await _context.Ratings
            .Where(rating => rating.MovieId == movieId)
            .AverageAsync(rating => rating.Score);
    }

    public async Task AddRatingToMovie(int movieId, Rating rating)
    {
        var existingRating = await _context.Ratings
            .FirstOrDefaultAsync(r => r.MovieId == movieId && r.AppUserId == rating.AppUserId);

        if (existingRating != null)
        {
            existingRating.Score = rating.Score;
        }
        else
        {
            _context.Ratings.Add(rating);
        }
        await _context.SaveChangesAsync();
    }


    public async Task UpdateRatingForMovie(int movieId, Rating rating)
    {
        var existingRating = await _context.Ratings
            .FirstOrDefaultAsync(r => r.MovieId == movieId && r.AppUserId == rating.AppUserId);

        if (existingRating != null)
        {
            existingRating.Score = rating.Score;
            await _context.SaveChangesAsync();
        }
    }

}
