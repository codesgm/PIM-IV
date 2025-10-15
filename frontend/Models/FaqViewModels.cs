using System.ComponentModel.DataAnnotations;

namespace PimWeb.Models
{
    public class FaqListViewModel
    {
        public List<FaqResponseDto> Faqs { get; set; } = new List<FaqResponseDto>();
        public string? TermoBusca { get; set; }
    }

    public class FaqCadastroViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Pergunta é obrigatória")]
        [StringLength(500, ErrorMessage = "Pergunta deve ter no máximo 500 caracteres")]
        [Display(Name = "Pergunta")]
        public string Pergunta { get; set; } = string.Empty;

        [Required(ErrorMessage = "Resposta é obrigatória")]
        [Display(Name = "Resposta")]
        public string Resposta { get; set; } = string.Empty;

        [Required(ErrorMessage = "Categoria é obrigatória")]
        [Display(Name = "Categoria")]
        public CategoriaFaq Categoria { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        public bool IsEdit => Id.HasValue;
    }

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
        public string Pergunta { get; set; } = string.Empty;
        public string Resposta { get; set; } = string.Empty;
        public CategoriaFaq Categoria { get; set; }
    }

    public class EditarFaqRequestDto
    {
        public string Pergunta { get; set; } = string.Empty;
        public string Resposta { get; set; } = string.Empty;
        public CategoriaFaq Categoria { get; set; }
        public bool Ativo { get; set; } = true;
    }

    public enum CategoriaFaq
    {
        GestaoContas = 1,
        Relatorios = 2,
        Configuracoes = 3,
        ProblemasTecnicos = 4
    }
}
