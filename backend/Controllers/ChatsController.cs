using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PimApi.Data;
using PimApi.DTOs;
using PimApi.Models;
using PimApi.Services;

namespace PimApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IChatAssignmentService _assignmentService;

        public ChatsController(AppDbContext context, IChatAssignmentService assignmentService)
        {
            _context = context;
            _assignmentService = assignmentService;
        }

        // GET: api/chats
        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<ChatResponseDto>>>> GetChats()
        {
            try
            {
                var chats = await _context.Chats
                    .Include(c => c.AssignedTechnician)
                    .Include(c => c.Messages)
                    .OrderByDescending(c => c.UpdatedAt)
                    .Select(c => new ChatResponseDto
                    {
                        Id = c.Id,
                        UserName = c.UserName,
                        UserContact = c.UserContact,
                        UserEmail = c.UserEmail,
                        InitialMessage = c.InitialMessage,
                        Status = c.Status.ToString(),
                        Source = c.Source,
                        AssignedTechnicianId = c.AssignedTechnicianId,
                        AssignedTechnicianName = c.AssignedTechnician != null ? c.AssignedTechnician.Nome : null,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        ResolvedAt = c.ResolvedAt,
                        MessagesCount = c.Messages.Count
                    })
                    .ToListAsync();

                return Ok(ApiResponseDto<List<ChatResponseDto>>.SuccessResult(chats));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDto<List<ChatResponseDto>>.ErrorResult($"Erro interno: {ex.Message}"));
            }
        }

        // GET: api/chats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<ChatResponseDto>>> GetChat(int id)
        {
            try
            {
                var chat = await _context.Chats
                    .Include(c => c.AssignedTechnician)
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (chat == null)
                {
                    return NotFound(ApiResponseDto<ChatResponseDto>.ErrorResult("Chat não encontrado"));
                }

                var chatDto = new ChatResponseDto
                {
                    Id = chat.Id,
                    UserName = chat.UserName,
                    UserContact = chat.UserContact,
                    InitialMessage = chat.InitialMessage,
                    Status = chat.Status.ToString(),
                    Source = chat.Source,
                    AssignedTechnicianId = chat.AssignedTechnicianId,
                    AssignedTechnicianName = chat.AssignedTechnician?.Nome,
                    CreatedAt = chat.CreatedAt,
                    UpdatedAt = chat.UpdatedAt,
                    ResolvedAt = chat.ResolvedAt,
                    MessagesCount = chat.Messages.Count
                };

                return Ok(ApiResponseDto<ChatResponseDto>.SuccessResult(chatDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDto<ChatResponseDto>.ErrorResult($"Erro interno: {ex.Message}"));
            }
        }

        // POST: api/chats
        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<ChatResponseDto>>> CreateChat([FromBody] CreateChatRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponseDto<ChatResponseDto>.ErrorResult("Dados inválidos"));
                }

                Console.WriteLine($"[DEBUG] Criando chat - Nome: {request.UserName}, Email: {request.UserEmail}");

                var chat = new Chat
                {
                    UserName = request.UserName,
                    UserContact = request.UserContact,
                    UserEmail = request.UserEmail,
                    InitialMessage = request.InitialMessage,
                    Source = request.Source,
                    Status = ChatStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();

                Console.WriteLine($"[DEBUG] Chat criado com ID: {chat.Id}");

                // Tentar atribuir a um técnico
                Console.WriteLine($"[DEBUG] Tentando atribuir chat {chat.Id} a um técnico...");
                var assignedTechnicianId = await _assignmentService.AssignChatToTechnician(chat.Id);
                Console.WriteLine($"[DEBUG] Resultado da atribuição: {assignedTechnicianId}");
                
                // Recarregar chat com técnico atribuído se houver
                if (assignedTechnicianId.HasValue)
                {
                    chat = await _context.Chats
                        .Include(c => c.AssignedTechnician)
                        .FirstOrDefaultAsync(c => c.Id == chat.Id);
                    Console.WriteLine($"[DEBUG] Chat recarregado com técnico: {chat?.AssignedTechnician?.Nome}");
                }

                // Adicionar mensagem inicial
                var initialMessage = new ChatMessage
                {
                    ChatId = chat.Id,
                    SenderType = "user",
                    Message = request.InitialMessage,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ChatMessages.Add(initialMessage);
                await _context.SaveChangesAsync();

                var chatDto = new ChatResponseDto
                {
                    Id = chat.Id,
                    UserName = chat.UserName,
                    UserContact = chat.UserContact,
                    UserEmail = chat.UserEmail,
                    InitialMessage = chat.InitialMessage,
                    Status = chat.Status.ToString(),
                    Source = chat.Source,
                    AssignedTechnicianId = chat.AssignedTechnicianId,
                    AssignedTechnicianName = chat.AssignedTechnician?.Nome,
                    CreatedAt = chat.CreatedAt,
                    UpdatedAt = chat.UpdatedAt,
                    MessagesCount = 1
                };

                Console.WriteLine($"[DEBUG] Retornando chat DTO com email: {chatDto.UserEmail}");

                return Ok(ApiResponseDto<ChatResponseDto>.SuccessResult(chatDto));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Erro ao criar chat: {ex.Message}");
                return StatusCode(500, ApiResponseDto<ChatResponseDto>.ErrorResult($"Erro interno: {ex.Message}"));
            }
        }

        // GET: api/chats/5/messages
        [HttpGet("{id}/messages")]
        public async Task<ActionResult<ApiResponseDto<List<ChatMessageResponseDto>>>> GetChatMessages(int id, [FromQuery] int? afterId = null)
        {
            try
            {
                var chatExists = await _context.Chats.AnyAsync(c => c.Id == id);
                if (!chatExists)
                {
                    return NotFound(ApiResponseDto<List<ChatMessageResponseDto>>.ErrorResult("Chat não encontrado"));
                }

                var query = _context.ChatMessages
                    .Include(m => m.Sender)
                    .Where(m => m.ChatId == id);

                if (afterId.HasValue)
                {
                    query = query.Where(m => m.Id > afterId.Value);
                }

                var messages = await query
                    .OrderBy(m => m.CreatedAt)
                    .Select(m => new ChatMessageResponseDto
                    {
                        Id = m.Id,
                        ChatId = m.ChatId,
                        SenderType = m.SenderType,
                        SenderId = m.SenderId,
                        SenderName = m.Sender != null ? m.Sender.Nome : null,
                        Message = m.Message,
                        CreatedAt = m.CreatedAt
                    })
                    .ToListAsync();

                return Ok(ApiResponseDto<List<ChatMessageResponseDto>>.SuccessResult(messages));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDto<List<ChatMessageResponseDto>>.ErrorResult($"Erro interno: {ex.Message}"));
            }
        }

        // POST: api/chats/5/messages
        [HttpPost("{id}/messages")]
        public async Task<ActionResult<ApiResponseDto<ChatMessageResponseDto>>> SendMessage(int id, [FromBody] SendMessageRequestDto request)
        {
            try
            {
                var chat = await _context.Chats.FindAsync(id);
                if (chat == null)
                {
                    return NotFound(ApiResponseDto<ChatMessageResponseDto>.ErrorResult("Chat não encontrado"));
                }

                var message = new ChatMessage
                {
                    ChatId = id,
                    SenderType = request.SenderType,
                    SenderId = request.SenderId,
                    Message = request.Message,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ChatMessages.Add(message);
                
                // Atualizar timestamp do chat
                chat.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();

                // Buscar dados do sender se existir
                Usuario? sender = null;
                if (request.SenderId.HasValue)
                {
                    sender = await _context.Usuarios.FindAsync(request.SenderId.Value);
                }

                var messageDto = new ChatMessageResponseDto
                {
                    Id = message.Id,
                    ChatId = message.ChatId,
                    SenderType = message.SenderType,
                    SenderId = message.SenderId,
                    SenderName = sender?.Nome,
                    Message = message.Message,
                    CreatedAt = message.CreatedAt
                };

                return Ok(ApiResponseDto<ChatMessageResponseDto>.SuccessResult(messageDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDto<ChatMessageResponseDto>.ErrorResult($"Erro interno: {ex.Message}"));
            }
        }

        // PUT: api/chats/5/resolve
        [HttpPut("{id}/resolve")]
        public async Task<ActionResult<ApiResponseDto<ChatResponseDto>>> ResolveChat(int id)
        {
            try
            {
                var chat = await _context.Chats
                    .Include(c => c.AssignedTechnician)
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (chat == null)
                {
                    return NotFound(ApiResponseDto<ChatResponseDto>.ErrorResult("Chat não encontrado"));
                }

                chat.Status = ChatStatus.Resolved;
                chat.ResolvedAt = DateTime.UtcNow;
                chat.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var chatDto = new ChatResponseDto
                {
                    Id = chat.Id,
                    UserName = chat.UserName,
                    UserContact = chat.UserContact,
                    InitialMessage = chat.InitialMessage,
                    Status = chat.Status.ToString(),
                    Source = chat.Source,
                    AssignedTechnicianId = chat.AssignedTechnicianId,
                    AssignedTechnicianName = chat.AssignedTechnician?.Nome,
                    CreatedAt = chat.CreatedAt,
                    UpdatedAt = chat.UpdatedAt,
                    ResolvedAt = chat.ResolvedAt,
                    MessagesCount = chat.Messages.Count
                };

                return Ok(ApiResponseDto<ChatResponseDto>.SuccessResult(chatDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDto<ChatResponseDto>.ErrorResult($"Erro interno: {ex.Message}"));
            }
        }
    }
}
