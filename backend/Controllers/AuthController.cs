using Microsoft.AspNetCore.Mvc;
using PimApi.DTOs;
using PimApi.Services;

namespace PimApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);
                
                if (result == null)
                {
                    return Ok(new { success = false, message = "Credenciais inválidas" });
                }

                return Ok(new { success = true, message = "Login realizado com sucesso", data = result });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = $"Erro: {ex.Message}" });
            }
        }
    }
}
