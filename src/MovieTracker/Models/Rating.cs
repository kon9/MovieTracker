namespace MovieTracker.Models;

public class Rating
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}