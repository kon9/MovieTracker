namespace MovieTracker.Core.DTOs
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int MovieId { get; set; }
        public int Score { get; set; }
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    
    public class CreateRatingDto
    {
        public int MovieId { get; set; }
        public int Score { get; set; }
        public string? Review { get; set; }
    }
    
    public class UpdateRatingDto
    {
        public int Score { get; set; }
        public string? Review { get; set; }
    }
} 