using System.ComponentModel.DataAnnotations;

namespace PimApi.Models
{
    public class Chat
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string UserContact { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string UserEmail { get; set; } = string.Empty;
        
        [Required]
        public string InitialMessage { get; set; } = string.Empty;
        
        public ChatStatus Status { get; set; } = ChatStatus.Active;
        
        [MaxLength(20)]
        public string Source { get; set; } = "faq";
        
        public int? AssignedTechnicianId { get; set; }
        public Usuario? AssignedTechnician { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }
        
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
    
    public enum ChatStatus
    {
        Active = 1,
        Resolved = 2,
        Closed = 3
    }
}
