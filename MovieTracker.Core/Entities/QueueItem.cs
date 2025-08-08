using System.ComponentModel.DataAnnotations;

namespace MovieTracker.Core.Entities
{
    public class QueueItem
    {
        public int Id { get; set; }
        
        public int QueueId { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [StringLength(100)]
        public string? Type { get; set; } // "movie", "book", "game", etc.
        
        [StringLength(500)]
        public string? ExternalId { get; set; } // ID from external service (IMDB, etc.)
        
        [StringLength(500)]
        public string? ImageUrl { get; set; }
        
        public int Position { get; set; }
        
        public QueueItemStatus Status { get; set; } = QueueItemStatus.Pending;
        
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? CompletedAt { get; set; }
        
        public int? AddedById { get; set; }
        
        // Navigation properties
        public virtual Queue Queue { get; set; } = null!;
        public virtual User? AddedBy { get; set; }
    }
    
    public enum QueueItemStatus
    {
        Pending,
        InProgress,
        Completed,
        Skipped
    }
} 