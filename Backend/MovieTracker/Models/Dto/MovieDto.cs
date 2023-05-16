namespace MovieTracker.Models.Dto;

public class MovieDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public List<RatingDto>? Ratings { get; set; } = null;
    public List<CommentDto>? Comments { get; set; } = null;
}