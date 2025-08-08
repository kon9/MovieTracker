namespace MovieTracker.Core.DTOs
{
    public class QueueDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public int OwnerId { get; set; }
        public string OwnerUsername { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int TotalItems { get; set; }
        public int CompletedItems { get; set; }
        public List<QueueItemDto> Items { get; set; } = new List<QueueItemDto>();
        public List<QueueMemberDto> Members { get; set; } = new List<QueueMemberDto>();
    }
    
    public class CreateQueueDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsPublic { get; set; } = false;
    }
    
    public class UpdateQueueDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsPublic { get; set; }
    }
    
    public class QueueItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? ExternalId { get; set; }
        public string? ImageUrl { get; set; }
        public int Position { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime AddedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? AddedByUsername { get; set; }
    }
    
    public class AddItemToQueueDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? ExternalId { get; set; }
        public string? ImageUrl { get; set; }
        public int? Position { get; set; }
    }
    
    public class UpdateQueueItemDto
    {
        public string? Status { get; set; }
        public int? Position { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
    
    public class QueueMemberDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }
    
    public class AddMemberToQueueDto
    {
        public int UserId { get; set; }
        public string Role { get; set; } = "Member";
    }
} 