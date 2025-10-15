namespace PimWeb.Models
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioResponseDto Usuario { get; set; } = new();
    }
}
