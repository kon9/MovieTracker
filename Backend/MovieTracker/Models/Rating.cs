using MovieTracker.Models;
using System.Text.Json.Serialization;

namespace MovieTracker;

public class Rating
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int MovieId { get; set; }
    [JsonIgnore]
    public Movie Movie { get; set; }
    public string AppUserId { get; set; } // Add this
    public AppUser AppUser { get; set; } // And this
}