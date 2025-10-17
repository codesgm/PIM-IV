using PimWeb.Models;

namespace PimWeb.Services
{
    public class ChatService
    {
        private readonly ApiService _apiService;

        public ChatService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponseDto<List<ChatResponseDto>>?> GetChatsAsync()
        {
            return await _apiService.GetAsync<List<ChatResponseDto>>("api/chats");
        }

        public async Task<ApiResponseDto<ChatResponseDto>?> GetChatByIdAsync(int id)
        {
            return await _apiService.GetAsync<ChatResponseDto>($"api/chats/{id}");
        }

        public async Task<ApiResponseDto<List<ChatMessageResponseDto>>?> GetChatMessagesAsync(int chatId, int? afterId = null)
        {
            var endpoint = $"api/chats/{chatId}/messages";
            if (afterId.HasValue)
            {
                endpoint += $"?afterId={afterId}";
            }
            return await _apiService.GetAsync<List<ChatMessageResponseDto>>(endpoint);
        }

        public async Task<ApiResponseDto<ChatMessageResponseDto>?> SendMessageAsync(int chatId, string message, int senderId)
        {
            var request = new
            {
                message = message,
                senderType = "technician",
                senderId = senderId
            };
            return await _apiService.PostAsync<ChatMessageResponseDto>($"api/chats/{chatId}/messages", request);
        }

        public async Task<ApiResponseDto<ChatResponseDto>?> ResolveChatAsync(int chatId)
        {
            return await _apiService.PutAsync<ChatResponseDto>($"api/chats/{chatId}/resolve", new { });
        }
    }
}
