namespace PimWeb.Models
{
    public class ChatsViewModel
    {
        public bool IsAdmin { get; set; }
        public int CurrentUserId { get; set; }
        public string CurrentUserName { get; set; } = string.Empty;
        public List<ChatItemViewModel> Chats { get; set; } = new List<ChatItemViewModel>();
    }

    public class ChatItemViewModel
    {
        public int Id { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public string UltimaMensagem { get; set; } = string.Empty;
        public DateTime UltimaAtividade { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? TecnicoId { get; set; }
        public string? TecnicoNome { get; set; }
        public bool NaoLida { get; set; }
    }
}
