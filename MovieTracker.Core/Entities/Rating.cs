using System.ComponentModel.DataAnnotations;

namespace MovieTracker.Core.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int MovieId { get; set; }
        
        [Range(1, 10)]
        public int Score { get; set; }
        
        [StringLength(500)]
        public string? Review { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Movie Movie { get; set; } = null!;
    }
} 