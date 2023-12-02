namespace MovieTracker.Models;

public class CommentRating
{
    public int Id { get; set; }
    public int CommentId { get; set; }
    public Comment Comment { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public bool IsUpvote { get; set; }
}