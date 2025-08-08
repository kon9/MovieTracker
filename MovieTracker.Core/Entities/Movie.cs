using System.ComponentModel.DataAnnotations;

namespace MovieTracker.Core.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [StringLength(100)]
        public string? Director { get; set; }
        
        [StringLength(255)]
        public string? Genre { get; set; }
        
        public int? ReleaseYear { get; set; }
        
        [StringLength(50)]
        public string? Rating { get; set; } // PG, PG-13, R, etc.
        
        public int? Runtime { get; set; } // in minutes
        
        [StringLength(500)]
        public string? PosterUrl { get; set; }
        
        [StringLength(500)]
        public string? TrailerUrl { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
} 