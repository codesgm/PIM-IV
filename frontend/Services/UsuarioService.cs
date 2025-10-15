using PimWeb.Models;

namespace PimWeb.Services
{
    public class UsuarioService
    {
        private readonly ApiService _apiService;

        public UsuarioService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponseDto<UsuarioResponseDto>?> CadastrarAsync(CadastroUsuarioRequestDto request)
        {
            return await _apiService.PostAsync<UsuarioResponseDto>("api/usuarios", request);
        }

        public async Task<ApiResponseDto<List<UsuarioResponseDto>>?> ListarAsync()
        {
            return await _apiService.GetAsync<List<UsuarioResponseDto>>("api/usuarios");
        }

        public async Task<ApiResponseDto<UsuarioResponseDto>?> ObterPorIdAsync(int id)
        {
            return await _apiService.GetAsync<UsuarioResponseDto>($"api/usuarios/{id}");
        }

        public async Task<ApiResponseDto<UsuarioResponseDto>?> EditarAsync(int id, EditarUsuarioRequestDto request)
        {
            return await _apiService.PutAsync<UsuarioResponseDto>($"api/usuarios/{id}", request);
        }

        public async Task<ApiResponseDto<object>?> DesativarAsync(int id)
        {
            return await _apiService.DeleteAsync($"api/usuarios/{id}");
        }

        public async Task<ApiResponseDto<object>?> ResetarSenhaAsync(int id)
        {
            return await _apiService.PostAsync<object>($"api/usuarios/{id}/reset-senha", new { });
        }
    }
}
