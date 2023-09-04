namespace MovieTracker.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int TotalRating { get; set; }
    public ICollection<CommentRating> CommentRatings { get; set; }
    public int? ParentCommentId { get; set; }
    public Comment ParentComment { get; set; }
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}