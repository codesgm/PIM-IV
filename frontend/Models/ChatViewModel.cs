using System.ComponentModel.DataAnnotations;

namespace PimWeb.Models
{
    public class ChatListViewModel
    {
        public List<ChatResponseDto> Chats { get; set; } = new List<ChatResponseDto>();
        public string? StatusFilter { get; set; }
        public string? SearchTerm { get; set; }
    }

    public class ChatDetailViewModel
    {
        public ChatResponseDto Chat { get; set; } = new ChatResponseDto();
        public List<ChatMessageResponseDto> Messages { get; set; } = new List<ChatMessageResponseDto>();
        public SendMessageViewModel SendMessage { get; set; } = new SendMessageViewModel();
    }

    public class SendMessageViewModel
    {
        [Required(ErrorMessage = "Mensagem é obrigatória")]
        [StringLength(1000, ErrorMessage = "Mensagem deve ter no máximo 1000 caracteres")]
        public string Message { get; set; } = string.Empty;
        
        public int ChatId { get; set; }
    }

    public class ChatResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserContact { get; set; } = string.Empty;
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
