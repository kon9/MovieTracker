namespace MovieTracker.Core.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Director { get; set; }
        public string? Genre { get; set; }
        public int? ReleaseYear { get; set; }
        public int? Rating { get; set; }
        public int? Runtime { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public double? AverageRating { get; set; }
        public int TotalRatings { get; set; }
    }
    
    public class CreateMovieDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Director { get; set; }
        public string? Genre { get; set; }
        public int? ReleaseYear { get; set; }
        public int? Rating { get; set; }
        public int? Runtime { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
    }
    
    public class UpdateMovieDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Director { get; set; }
        public string? Genre { get; set; }
        public int? ReleaseYear { get; set; }
        public int? Rating { get; set; }
        public int? Runtime { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
    }
} 