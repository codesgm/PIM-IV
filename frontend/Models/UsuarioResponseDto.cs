namespace PimWeb.Models
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public StatusUsuario Status { get; set; }
        public PerfilAcesso PerfilAcesso { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
