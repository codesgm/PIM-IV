namespace FaqPublico.Models
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

    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }

    public class FaqListViewModel
    {
        public List<FaqResponseDto> Faqs { get; set; } = new List<FaqResponseDto>();
        public string? TermoBusca { get; set; }
        public CategoriaFaq? CategoriaFiltro { get; set; }
        public Dictionary<CategoriaFaq, string> Categorias { get; set; } = new Dictionary<CategoriaFaq, string>
        {
            { CategoriaFaq.GestaoContas, "Gestão de Contas" },
            { CategoriaFaq.Relatorios, "Relatórios" },
            { CategoriaFaq.Configuracoes, "Configurações" },
            { CategoriaFaq.ProblemasTecnicos, "Problemas Técnicos" }
        };
    }

    public enum CategoriaFaq
    {
        GestaoContas = 1,
        Relatorios = 2,
        Configuracoes = 3,
        ProblemasTecnicos = 4
    }
}
