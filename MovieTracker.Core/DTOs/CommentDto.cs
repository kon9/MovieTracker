namespace MovieTracker.Core.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int MovieId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    
    public class CreateCommentDto
    {
        public int MovieId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
    
    public class UpdateCommentDto
    {
        public string Content { get; set; } = string.Empty;
    }
} 