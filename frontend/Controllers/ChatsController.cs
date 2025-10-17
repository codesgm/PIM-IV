using Microsoft.AspNetCore.Mvc;
using PimWeb.Services;
using PimWeb.Models;

namespace PimWeb.Controllers
{
    public class ChatsController : Controller
    {
        private readonly AuthService _authService;
        private readonly ApiService _apiService;

        public ChatsController(AuthService authService, ApiService apiService)
        {
            _authService = authService;
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var currentUser = _authService.GetCurrentUser();
            var isAdmin = _authService.IsAdmin();

            var viewModel = new ChatsViewModel
            {
                IsAdmin = isAdmin,
                CurrentUserId = currentUser?.Id ?? 0,
                CurrentUserName = currentUser?.Nome ?? ""
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("Chat/GetActiveChats")]
        public async Task<IActionResult> GetActiveChats()
        {
            try
            {
                if (!_authService.IsAuthenticated())
                    return Json(new { success = false, message = "Não autenticado" });

                var response = await _apiService.GetAsync<List<ChatResponseDto>>("api/chats?status=Active&pageSize=20");
                
                if (response?.Success == true && response.Data != null)
                {
                    // Buscar última mensagem para cada chat
                    foreach (var chat in response.Data)
                    {
                        try
                        {
                            var messagesResponse = await _apiService.GetAsync<List<ChatMessageResponseDto>>($"api/chats/{chat.Id}/messages");
                            if (messagesResponse?.Success == true && messagesResponse.Data?.Any() == true)
                            {
                                var lastMessage = messagesResponse.Data.OrderByDescending(m => m.CreatedAt).First();
                                chat.LastMessage = lastMessage.Message.Length > 50 
                                    ? lastMessage.Message.Substring(0, 50) + "..." 
                                    : lastMessage.Message;
                            }
                            else
                            {
                                chat.LastMessage = chat.InitialMessage.Length > 50 
                                    ? chat.InitialMessage.Substring(0, 50) + "..." 
                                    : chat.InitialMessage;
                            }
                        }
                        catch
                        {
                            // Se falhar, usar mensagem inicial
                            chat.LastMessage = chat.InitialMessage.Length > 50 
                                ? chat.InitialMessage.Substring(0, 50) + "..." 
                                : chat.InitialMessage;
                        }
                    }
                    
                    return Json(new { success = true, data = response.Data });
                }
                
                return Json(new { success = false, message = response?.Message ?? "Erro ao carregar chats" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
