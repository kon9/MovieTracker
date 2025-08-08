using System.ComponentModel.DataAnnotations;

namespace MovieTracker.Core.Entities
{
    public class Queue
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public bool IsPublic { get; set; } = false;
        
        public int OwnerId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User Owner { get; set; } = null!;
        public virtual ICollection<QueueItem> Items { get; set; } = new List<QueueItem>();
        public virtual ICollection<QueueMember> Members { get; set; } = new List<QueueMember>();
    }
} 