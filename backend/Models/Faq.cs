using System.ComponentModel.DataAnnotations;

namespace PimApi.Models
{
    public class Faq
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Pergunta { get; set; } = string.Empty;
        
        [Required]
        public string Resposta { get; set; } = string.Empty;
        
        [Required]
        public CategoriaFaq Categoria { get; set; }
        
        public bool Ativo { get; set; } = true;
        
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        
        public DateTime? DataAtualizacao { get; set; }
    }
    
    public enum CategoriaFaq
    {
        GestaoContas = 1,
        Relatorios = 2,
        Configuracoes = 3,
        ProblemasTecnicos = 4
    }
}
