using Microsoft.EntityFrameworkCore;
using MovieTracker.Infrastructure.Data;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;

namespace MovieTracker.Infrastructure.Repo;

public class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository
{
    public UserProfileRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task AddFavoriteMovieAsync(int userProfileId, int movieId)
    {
        var favoriteMovie = new FavoriteMovie { UserProfileId = userProfileId, MovieId = movieId };
        await _context.FavoriteMovies.AddAsync(favoriteMovie);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFavoriteMovieAsync(int userProfileId, int movieId)
    {
        var favoriteMovie = await _context.FavoriteMovies
            .FirstOrDefaultAsync(fm => fm.UserProfileId == userProfileId && fm.MovieId == movieId);

        if (favoriteMovie != null)
        {
            _context.FavoriteMovies.Remove(favoriteMovie);
            await _context.SaveChangesAsync();
        }
    }
}