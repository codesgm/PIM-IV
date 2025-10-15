using PimWeb.Models;

namespace PimWeb.Services
{
    public class FaqService
    {
        private readonly ApiService _apiService;

        public FaqService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponseDto<List<FaqResponseDto>>?> ListarFaqsAdminAsync()
        {
            return await _apiService.GetAsync<List<FaqResponseDto>>("api/faq/admin");
        }

        public async Task<ApiResponseDto<List<FaqResponseDto>>?> ListarFaqsAsync()
        {
            return await _apiService.GetAsync<List<FaqResponseDto>>("api/faq");
        }

        public async Task<ApiResponseDto<FaqResponseDto>?> ObterFaqPorIdAsync(int id)
        {
            return await _apiService.GetAsync<FaqResponseDto>($"api/faq/{id}");
        }

        public async Task<ApiResponseDto<FaqResponseDto>?> CriarFaqAsync(CriarFaqRequestDto request)
        {
            return await _apiService.PostAsync<FaqResponseDto>("api/faq", request);
        }

        public async Task<ApiResponseDto<FaqResponseDto>?> EditarFaqAsync(int id, EditarFaqRequestDto request)
        {
            return await _apiService.PutAsync<FaqResponseDto>($"api/faq/{id}", request);
        }

        public async Task<ApiResponseDto<object>?> ExcluirFaqAsync(int id)
        {
            return await _apiService.DeleteAsync($"api/faq/{id}");
        }

        public async Task<ApiResponseDto<List<FaqResponseDto>>?> BuscarFaqsAsync(string termo)
        {
            return await _apiService.GetAsync<List<FaqResponseDto>>($"api/faq/buscar?termo={Uri.EscapeDataString(termo)}");
        }
    }
}
