using System.ComponentModel.DataAnnotations;

namespace PimApi.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        
        public int ChatId { get; set; }
        public Chat Chat { get; set; } = null!;
        
        [Required]
        [MaxLength(20)]
        public string SenderType { get; set; } = string.Empty; // "user" ou "technician"
        
        public int? SenderId { get; set; } // ID do técnico se for technician
        public Usuario? Sender { get; set; }
        
        [Required]
        public string Message { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
