namespace MovieTracker.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public ICollection<FavoriteMovie> FavoriteMovies { get; set; }
}