using Microsoft.AspNetCore.Mvc;
using PimWeb.Models;
using PimWeb.Services;

namespace PimWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_authService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loginRequest = new LoginRequestDto
            {
                Email = model.Email,
                Senha = model.Senha
            };

            var response = await _authService.LoginAsync(loginRequest);

            if (response?.Success == true && response.Data != null)
            {
                _authService.SaveUserSession(response.Data);
                return RedirectToAction("Index", "Home");
            }

            model.ErrorMessage = response?.Message ?? "Erro ao fazer login";
            return View(model);
        }

        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Login");
        }
    }
}
