using Microsoft.EntityFrameworkCore;
using PimApi.Data;
using PimApi.DTOs;
using PimApi.Models;

namespace PimApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Status == StatusUsuario.Ativo);

            if (usuario == null || usuario.Senha != dto.Senha)
            {
                return null;
            }

            var token = GerarToken(usuario);
            
            return new LoginResponseDto
            {
                Token = token,
                Usuario = new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Status = usuario.Status,
                    PerfilAcesso = usuario.PerfilAcesso,
                    DataCadastro = usuario.DataCadastro
                }
            };
        }

        public string GerarToken(Usuario usuario)
        {
            return $"token-{usuario.Id}-{usuario.PerfilAcesso}";
        }
    }
}
