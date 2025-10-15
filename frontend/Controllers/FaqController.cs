using Microsoft.AspNetCore.Mvc;
using PimWeb.Services;
using PimWeb.Models;

namespace PimWeb.Controllers
{
    public class FaqController : Controller
    {
        private readonly FaqService _faqService;
        private readonly AuthService _authService;

        public FaqController(FaqService faqService, AuthService authService)
        {
            _faqService = faqService;
            _authService = authService;
        }

        public async Task<IActionResult> Index(string? busca)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_authService.IsAdmin())
                return Forbid();

            try
            {
                ApiResponseDto<List<FaqResponseDto>>? response;
                
                if (!string.IsNullOrWhiteSpace(busca))
                {
                    response = await _faqService.BuscarFaqsAsync(busca);
                }
                else
                {
                    response = await _faqService.ListarFaqsAdminAsync();
                }

                var viewModel = new FaqListViewModel
                {
                    Faqs = response?.Data ?? new List<FaqResponseDto>(),
                    TermoBusca = busca
                };

                return View(viewModel);
            }
            catch
            {
                var viewModel = new FaqListViewModel();
                ViewBag.ErrorMessage = "Erro ao carregar FAQs";
                return View(viewModel);
            }
        }

        public IActionResult Criar()
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_authService.IsAdmin())
                return Forbid();

            return View(new FaqCadastroViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Criar(FaqCadastroViewModel model)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_authService.IsAdmin())
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var dto = new CriarFaqRequestDto
                {
                    Pergunta = model.Pergunta,
                    Resposta = model.Resposta,
                    Categoria = model.Categoria
                };

                var response = await _faqService.CriarFaqAsync(dto);
                
                if (response?.Success == true)
                {
                    TempData["SuccessMessage"] = "FAQ criado com sucesso!";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", response?.Message ?? "Erro ao criar FAQ");
                return View(model);
            }
            catch
            {
                ModelState.AddModelError("", "Erro interno do servidor");
                return View(model);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_authService.IsAdmin())
                return Forbid();

            try
            {
                var response = await _faqService.ObterFaqPorIdAsync(id);
                
                if (response?.Data == null)
                    return NotFound();

                var faq = response.Data;
                var viewModel = new FaqCadastroViewModel
                {
                    Id = faq.Id,
                    Pergunta = faq.Pergunta,
                    Resposta = faq.Resposta,
                    Categoria = faq.Categoria,
                    Ativo = faq.Ativo
                };

                return View(viewModel);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(FaqCadastroViewModel model)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_authService.IsAdmin())
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var dto = new EditarFaqRequestDto
                {
                    Pergunta = model.Pergunta,
                    Resposta = model.Resposta,
                    Categoria = model.Categoria,
                    Ativo = model.Ativo
                };

                var response = await _faqService.EditarFaqAsync(model.Id ?? 0, dto);
                
                if (response?.Success == true)
                {
                    TempData["SuccessMessage"] = "FAQ editado com sucesso!";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", response?.Message ?? "Erro ao editar FAQ");
                return View(model);
            }
            catch
            {
                ModelState.AddModelError("", "Erro interno do servidor");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_authService.IsAdmin())
                return Forbid();

            try
            {
                var response = await _faqService.ExcluirFaqAsync(id);
                
                if (response?.Success == true)
                {
                    TempData["SuccessMessage"] = "FAQ excluído com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Erro ao excluir FAQ";
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Erro interno do servidor";
            }

            return RedirectToAction("Index");
        }
    }
}
