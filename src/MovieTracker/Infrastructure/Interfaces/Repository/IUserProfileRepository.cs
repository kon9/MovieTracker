using MovieTracker.Models;

namespace MovieTracker.Infrastructure.Interfaces;

public interface IUserProfileRepository : IRepository<UserProfile>
{
    Task AddFavoriteMovieAsync(int userProfileId, int movieId);
    Task RemoveFavoriteMovieAsync(int userProfileId, int movieId);
}