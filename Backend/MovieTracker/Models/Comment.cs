using System.Text.Json.Serialization;

namespace MovieTracker.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int MovieId { get; set; }
    [JsonIgnore]
    public Movie Movie { get; set; }
}