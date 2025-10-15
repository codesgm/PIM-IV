using System.ComponentModel.DataAnnotations;

namespace PimApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Senha { get; set; } = string.Empty;
        
        public StatusUsuario Status { get; set; } = StatusUsuario.Ativo;
        
        public PerfilAcesso PerfilAcesso { get; set; }
        
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        
        public DateTime? DataUltimaModificacao { get; set; }
    }
    
    public enum StatusUsuario
    {
        Ativo = 1,
        Inativo = 2
    }
    
    public enum PerfilAcesso
    {
        Administrador = 1,
        Tecnico = 2
    }
}
