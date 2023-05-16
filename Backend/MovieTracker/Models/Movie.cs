namespace MovieTracker.Models;

public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Rating> Ratings { get; set; }
}