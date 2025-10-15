using Microsoft.AspNetCore.Mvc;
using FaqPublico.Models;
using FaqPublico.Services;

namespace FaqPublico.Controllers
{
    public class HomeController : Controller
    {
        private readonly FaqApiService _faqService;

        public HomeController(FaqApiService faqService)
        {
            _faqService = faqService;
        }

        public async Task<IActionResult> Index(string? busca, int? categoria)
        {
            var viewModel = new FaqListViewModel
            {
                TermoBusca = busca,
                CategoriaFiltro = categoria.HasValue ? (CategoriaFaq)categoria.Value : null
            };

            try
            {
                if (!string.IsNullOrWhiteSpace(busca))
                {
                    viewModel.Faqs = await _faqService.BuscarFaqsAsync(busca);
                }
                else if (categoria.HasValue)
                {
                    viewModel.Faqs = await _faqService.ListarFaqsPorCategoriaAsync((CategoriaFaq)categoria.Value);
                }
                else
                {
                    viewModel.Faqs = await _faqService.ListarFaqsAsync();
                }
            }
            catch
            {
                ViewBag.ErrorMessage = "Erro ao carregar FAQs. Tente novamente.";
            }

            return View(viewModel);
        }
    }
}
