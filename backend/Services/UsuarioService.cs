using Microsoft.EntityFrameworkCore;
using PimApi.Data;
using PimApi.DTOs;
using PimApi.Models;

namespace PimApi.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioResponseDto> CadastrarAsync(CadastroUsuarioRequestDto dto)
        {
            // Verificar se email já existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                throw new InvalidOperationException("Email já existe");
            }

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = "123", // Senha padrão em texto plano
                Status = StatusUsuario.Ativo,
                PerfilAcesso = dto.PerfilAcesso,
                DataCadastro = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return MapToResponseDto(usuario);
        }

        public async Task<List<UsuarioResponseDto>> ListarAsync(string? status = null, string? perfilAcesso = null, string? busca = null, int page = 1, int pageSize = 10)
        {
            var query = _context.Usuarios.AsQueryable();

            // Filtros
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<StatusUsuario>(status, out var statusEnum))
            {
                query = query.Where(u => u.Status == statusEnum);
            }

            if (!string.IsNullOrEmpty(perfilAcesso) && Enum.TryParse<PerfilAcesso>(perfilAcesso, out var perfilEnum))
            {
                query = query.Where(u => u.PerfilAcesso == perfilEnum);
            }

            if (!string.IsNullOrEmpty(busca))
            {
                query = query.Where(u => u.Nome.Contains(busca) || u.Email.Contains(busca));
            }

            // Paginação
            var usuarios = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return usuarios.Select(MapToResponseDto).ToList();
        }

        public async Task<UsuarioResponseDto?> ObterPorIdAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            return usuario != null ? MapToResponseDto(usuario) : null;
        }

        public async Task<UsuarioResponseDto> EditarAsync(int id, EditarUsuarioRequestDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                throw new InvalidOperationException("Usuário não encontrado");
            }

            // Verificar se email já existe (exceto para o próprio usuário)
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id))
            {
                throw new InvalidOperationException("Email já existe");
            }

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;
            usuario.Status = dto.Status;
            usuario.PerfilAcesso = dto.PerfilAcesso;
            usuario.DataUltimaModificacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToResponseDto(usuario);
        }

        public async Task<bool> DesativarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return false;
            }

            usuario.Status = StatusUsuario.Inativo;
            usuario.DataUltimaModificacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResetarSenhaAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return false;
            }

            usuario.Senha = "123";
            usuario.DataUltimaModificacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        private static UsuarioResponseDto MapToResponseDto(Usuario usuario)
        {
            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Status = usuario.Status,
                PerfilAcesso = usuario.PerfilAcesso,
                DataCadastro = usuario.DataCadastro
            };
        }
    }
}
