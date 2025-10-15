using System.ComponentModel.DataAnnotations;

namespace PimWeb.Models
{
    public class UsuarioCadastroViewModel
    {
        public int? Id { get; set; }
        
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        
        [Display(Name = "Status")]
        public StatusUsuario Status { get; set; } = StatusUsuario.Ativo;
        
        [Required(ErrorMessage = "Perfil de acesso é obrigatório")]
        [Display(Name = "Perfil de Acesso")]
        public PerfilAcesso PerfilAcesso { get; set; }
        
        public bool IsEdit => Id.HasValue;
    }
}
