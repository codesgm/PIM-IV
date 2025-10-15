using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PimWeb.Models;
using PimWeb.Services;

namespace PimWeb.Controllers;

public class HomeController : Controller
{
    private readonly AuthService _authService;

    public HomeController(AuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Index()
    {
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Login", "Auth");
        }

        var user = _authService.GetCurrentUser();
        var model = new DashboardViewModel
        {
            NomeUsuario = user?.Nome ?? "Usuário",
            PerfilUsuario = user?.PerfilAcesso ?? PerfilAcesso.Tecnico
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
