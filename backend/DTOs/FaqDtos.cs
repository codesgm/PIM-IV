using System.ComponentModel.DataAnnotations;
using PimApi.Models;

namespace PimApi.DTOs
{
    public class FaqResponseDto
    {
        public int Id { get; set; }
        public string Pergunta { get; set; } = string.Empty;
        public string Resposta { get; set; } = string.Empty;
        public CategoriaFaq Categoria { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class CriarFaqRequestDto
    {
        [Required(ErrorMessage = "Pergunta é obrigatória")]
        [StringLength(500, ErrorMessage = "Pergunta deve ter no máximo 500 caracteres")]
        public string Pergunta { get; set; } = string.Empty;

        [Required(ErrorMessage = "Resposta é obrigatória")]
        public string Resposta { get; set; } = string.Empty;

        [Required(ErrorMessage = "Categoria é obrigatória")]
        public CategoriaFaq Categoria { get; set; }
    }

    public class EditarFaqRequestDto
    {
        [Required(ErrorMessage = "Pergunta é obrigatória")]
        [StringLength(500, ErrorMessage = "Pergunta deve ter no máximo 500 caracteres")]
        public string Pergunta { get; set; } = string.Empty;

        [Required(ErrorMessage = "Resposta é obrigatória")]
        public string Resposta { get; set; } = string.Empty;

        [Required(ErrorMessage = "Categoria é obrigatória")]
        public CategoriaFaq Categoria { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
