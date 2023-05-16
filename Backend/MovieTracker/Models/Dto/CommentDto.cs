namespace MovieTracker.Models.Dto;

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int MovieId { get; set; }
}