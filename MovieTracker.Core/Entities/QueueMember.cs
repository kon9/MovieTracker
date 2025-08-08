using System.ComponentModel.DataAnnotations;

namespace MovieTracker.Core.Entities
{
    public class QueueMember
    {
        public int Id { get; set; }
        
        public int QueueId { get; set; }
        
        public int UserId { get; set; }
        
        public QueueMemberRole Role { get; set; } = QueueMemberRole.Member;
        
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Queue Queue { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
    
    public enum QueueMemberRole
    {
        Owner,
        Admin,
        Member
    }
} 