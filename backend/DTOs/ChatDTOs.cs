using System.ComponentModel.DataAnnotations;

namespace PimApi.DTOs
{
    public class CreateChatRequestDto
    {
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
        
        [MaxLength(20)]
        public string Source { get; set; } = "faq";
    }

    public class SendMessageRequestDto
    {
        public string Message { get; set; } = string.Empty;
        public string SenderType { get; set; } = string.Empty;
        public int? SenderId { get; set; }
    }

    public class ChatResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserContact { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string InitialMessage { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public int? AssignedTechnicianId { get; set; }
        public string? AssignedTechnicianName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public int MessagesCount { get; set; }
    }

    public class ChatMessageResponseDto
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string SenderType { get; set; } = string.Empty;
        public int? SenderId { get; set; }
        public string? SenderName { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
