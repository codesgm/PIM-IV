using System.ComponentModel.DataAnnotations;
using PimApi.Models;

namespace PimApi.DTOs
{
    public class CadastroUsuarioRequestDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Perfil de acesso é obrigatório")]
        public PerfilAcesso PerfilAcesso { get; set; }
    }
}
