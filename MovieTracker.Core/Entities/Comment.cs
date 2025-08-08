using System.ComponentModel.DataAnnotations;

namespace MovieTracker.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int MovieId { get; set; }
        
        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Movie Movie { get; set; } = null!;
    }
} 