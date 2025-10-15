using Microsoft.AspNetCore.Mvc;
using PimWeb.Services;
using PimWeb.Models;

namespace PimWeb.Controllers
{
    public class ChatsController : Controller
    {
        private readonly AuthService _authService;

        public ChatsController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            if (!_authService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var currentUser = _authService.GetCurrentUser();
            var isAdmin = _authService.IsAdmin();

            var viewModel = new ChatsViewModel
            {
                IsAdmin = isAdmin,
                CurrentUserId = currentUser?.Id ?? 0,
                CurrentUserName = currentUser?.Nome ?? ""
            };

            return View(viewModel);
        }
    }
}
