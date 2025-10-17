using Microsoft.AspNetCore.Mvc;
using PimWeb.Services;
using PimWeb.Models;

namespace PimWeb.Controllers
{
    public class ChatController : Controller
    {
        private readonly ChatService _chatService;
        private readonly AuthService _authService;

        public ChatController(ChatService chatService, AuthService authService)
        {
            _chatService = chatService;
            _authService = authService;
        }

        public async Task<IActionResult> Index(string? status, string? search)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_authService.IsAdmin() && !_authService.IsTechnician())
                return Forbid();

            try
            {
                var response = await _chatService.GetChatsAsync();
                var chats = response?.Data ?? new List<ChatResponseDto>();

                // Filtros simples
                if (!string.IsNullOrEmpty(status))
                {
                    chats = chats.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(search))
                {
                    chats = chats.Where(c => c.UserName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                           c.InitialMessage.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                var viewModel = new ChatListViewModel
                {
                    Chats = chats,
                    StatusFilter = status,
                    SearchTerm = search
                };

                return View(viewModel);
            }
            catch
            {
                ViewBag.ErrorMessage = "Erro ao carregar chats";
                return View(new ChatListViewModel());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_authService.IsAdmin() && !_authService.IsTechnician())
                return Forbid();

            try
            {
                var chatResponse = await _chatService.GetChatByIdAsync(id);
                var messagesResponse = await _chatService.GetChatMessagesAsync(id);

                if (chatResponse?.Data == null)
                    return NotFound();

                var viewModel = new ChatDetailViewModel
                {
                    Chat = chatResponse.Data,
                    Messages = messagesResponse?.Data ?? new List<ChatMessageResponseDto>(),
                    SendMessage = new SendMessageViewModel { ChatId = id }
                };

                return View(viewModel);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int chatId, string message)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_authService.IsAdmin() && !_authService.IsTechnician())
                return Forbid();

            if (string.IsNullOrWhiteSpace(message))
            {
                TempData["ErrorMessage"] = "Mensagem não pode estar vazia";
                return RedirectToAction("Details", new { id = chatId });
            }

            try
            {
                var currentUser = _authService.GetCurrentUser();
                if (currentUser == null)
                    return RedirectToAction("Login", "Auth");

                var response = await _chatService.SendMessageAsync(chatId, message, currentUser.Id);

                if (response?.Success == true)
                {
                    TempData["SuccessMessage"] = "Mensagem enviada com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Erro ao enviar mensagem";
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Erro interno do servidor";
            }

            return RedirectToAction("Details", new { id = chatId });
        }

        [HttpPost]
        public async Task<IActionResult> Resolve(int id)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_authService.IsAdmin() && !_authService.IsTechnician())
                return Forbid();

            try
            {
                var response = await _chatService.ResolveChatAsync(id);

                if (response?.Success == true)
                {
                    TempData["SuccessMessage"] = "Chat resolvido com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Erro ao resolver chat";
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Erro interno do servidor";
            }

            return RedirectToAction("Details", new { id });
        }

        // AJAX endpoint para buscar novas mensagens
        [HttpGet]
        public async Task<IActionResult> GetNewMessages(int chatId, int lastMessageId)
        {
            if (!_authService.IsAuthenticated())
                return Unauthorized();

            try
            {
                var response = await _chatService.GetChatMessagesAsync(chatId, lastMessageId);
                var messages = response?.Data ?? new List<ChatMessageResponseDto>();

                return Json(new { success = true, messages });
            }
            catch
            {
                return Json(new { success = false, message = "Erro ao buscar mensagens" });
            }
        }
    }
}
