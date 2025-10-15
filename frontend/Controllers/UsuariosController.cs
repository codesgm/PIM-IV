using Microsoft.AspNetCore.Mvc;
using PimWeb.Services;
using PimWeb.Models;

namespace PimWeb.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _usuarioService;
        private readonly AuthService _authService;

        public UsuariosController(UsuarioService usuarioService, AuthService authService)
        {
            _usuarioService = usuarioService;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var response = await _usuarioService.ListarAsync();
            var usuarios = response?.Data ?? new List<UsuarioResponseDto>();
            var viewModel = new UsuarioListViewModel { Usuarios = usuarios };
            return View(viewModel);
        }

        public IActionResult Criar()
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            return View(new UsuarioCadastroViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Criar(UsuarioCadastroViewModel model)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(model);

            var dto = new CadastroUsuarioRequestDto
            {
                Nome = model.Nome,
                Email = model.Email,
                PerfilAcesso = model.PerfilAcesso
            };

            var response = await _usuarioService.CadastrarAsync(dto);
            if (response?.Success == true)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Erro ao criar usuário");
            return View(model);
        }

        public async Task<IActionResult> Editar(int id)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var response = await _usuarioService.ObterPorIdAsync(id);
            if (response?.Data == null)
                return NotFound();

            var usuario = response.Data;
            var viewModel = new UsuarioCadastroViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                PerfilAcesso = usuario.PerfilAcesso
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioCadastroViewModel model)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(model);

            var dto = new EditarUsuarioRequestDto
            {
                Nome = model.Nome,
                Email = model.Email,
                PerfilAcesso = model.PerfilAcesso
            };

            var response = await _usuarioService.EditarAsync(model.Id ?? 0, dto);
            if (response?.Success == true)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Erro ao editar usuário");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            await _usuarioService.DesativarAsync(id);
            return RedirectToAction("Index");
        }
    }
}
