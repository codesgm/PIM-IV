using Newtonsoft.Json;
using FaqPublico.Models;

namespace FaqPublico.Services
{
    public class FaqApiService
    {
        private readonly HttpClient _httpClient;

        public FaqApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            var baseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://api:5000";
            _httpClient.BaseAddress = new Uri($"{baseUrl}/");
        }

        public async Task<List<FaqResponseDto>> ListarFaqsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/faq");
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<List<FaqResponseDto>>>(content);
                return apiResponse?.Data ?? new List<FaqResponseDto>();
            }
            catch
            {
                return new List<FaqResponseDto>();
            }
        }

        public async Task<List<FaqResponseDto>> BuscarFaqsAsync(string termo)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/faq/buscar?termo={Uri.EscapeDataString(termo)}");
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<List<FaqResponseDto>>>(content);
                return apiResponse?.Data ?? new List<FaqResponseDto>();
            }
            catch
            {
                return new List<FaqResponseDto>();
            }
        }

        public async Task<List<FaqResponseDto>> ListarFaqsPorCategoriaAsync(CategoriaFaq categoria)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/faq/categoria/{(int)categoria}");
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<List<FaqResponseDto>>>(content);
                return apiResponse?.Data ?? new List<FaqResponseDto>();
            }
            catch
            {
                return new List<FaqResponseDto>();
            }
        }
    }
}
