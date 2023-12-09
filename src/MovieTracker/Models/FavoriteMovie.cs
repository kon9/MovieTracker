namespace MovieTracker.Models;

public class FavoriteMovie
{
    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}