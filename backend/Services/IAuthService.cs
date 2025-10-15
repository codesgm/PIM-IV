using PimApi.DTOs;
using PimApi.Models;

namespace PimApi.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
        string GerarToken(Usuario usuario);
    }
}
