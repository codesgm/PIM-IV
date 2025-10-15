namespace PimWeb.Models
{
    public class DashboardViewModel
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public PerfilAcesso PerfilUsuario { get; set; }
        public int TotalUsuarios { get; set; }
        public int UsuariosAtivos { get; set; }
        public int UsuariosInativos { get; set; }
    }
}
