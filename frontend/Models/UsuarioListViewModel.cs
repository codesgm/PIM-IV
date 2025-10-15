namespace PimWeb.Models
{
    public class UsuarioListViewModel
    {
        public List<UsuarioResponseDto> Usuarios { get; set; } = new();
        public string? FiltroStatus { get; set; }
        public string? FiltroPerfil { get; set; }
        public string? Busca { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
