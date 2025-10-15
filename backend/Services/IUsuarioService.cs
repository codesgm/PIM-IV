using PimApi.DTOs;

namespace PimApi.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioResponseDto> CadastrarAsync(CadastroUsuarioRequestDto dto);
        Task<List<UsuarioResponseDto>> ListarAsync(string? status = null, string? perfilAcesso = null, string? busca = null, int page = 1, int pageSize = 10);
        Task<UsuarioResponseDto?> ObterPorIdAsync(int id);
        Task<UsuarioResponseDto> EditarAsync(int id, EditarUsuarioRequestDto dto);
        Task<bool> DesativarAsync(int id);
        Task<bool> ResetarSenhaAsync(int id);
    }
}
