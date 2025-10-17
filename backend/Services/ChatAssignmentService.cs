using Microsoft.EntityFrameworkCore;
using PimApi.Data;
using PimApi.Models;

namespace PimApi.Services
{
    public class ChatAssignmentService : IChatAssignmentService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ChatAssignmentService> _logger;

        public ChatAssignmentService(AppDbContext context, ILogger<ChatAssignmentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Usuario>> GetAvailableTechnicians()
        {
            return await _context.Usuarios
                .Where(u => u.PerfilAcesso == PerfilAcesso.Tecnico && u.Status == StatusUsuario.Ativo)
                .OrderBy(u => u.Id) // Simples ordenação para round-robin
                .ToListAsync();
        }

        public async Task<int?> AssignChatToTechnician(int chatId)
        {
            try
            {
                var technicians = await GetAvailableTechnicians();
                
                if (!technicians.Any())
                {
                    _logger.LogWarning("Nenhum técnico disponível para atribuição do chat {ChatId}", chatId);
                    return null;
                }

                // Algoritmo round-robin simples
                // Buscar técnico com menos chats ativos
                var technicianWithLeastChats = await _context.Usuarios
                    .Where(u => technicians.Select(t => t.Id).Contains(u.Id))
                    .Select(u => new { 
                        TechnicianId = u.Id, 
                        ActiveChatsCount = _context.Chats.Count(c => c.AssignedTechnicianId == u.Id && c.Status == ChatStatus.Active)
                    })
                    .OrderBy(x => x.ActiveChatsCount)
                    .ThenBy(x => x.TechnicianId)
                    .FirstOrDefaultAsync();

                if (technicianWithLeastChats == null)
                {
                    return null;
                }

                // Atribuir chat ao técnico
                var chat = await _context.Chats.FindAsync(chatId);
                if (chat != null)
                {
                    chat.AssignedTechnicianId = technicianWithLeastChats.TechnicianId;
                    chat.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Chat {ChatId} atribuído ao técnico {TechnicianId}", 
                        chatId, technicianWithLeastChats.TechnicianId);
                    
                    return technicianWithLeastChats.TechnicianId;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atribuir chat {ChatId} a técnico", chatId);
                return null;
            }
        }
    }
}
