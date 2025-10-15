using Microsoft.AspNetCore.Mvc;
using PimApi.DTOs;

namespace PimApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerfisController : ControllerBase
    {
        [HttpGet]
        public ActionResult<ApiResponseDto<string[]>> GetPerfis()
        {
            var perfis = new[] { "Administrador", "Tecnico" };
            return Ok(ApiResponseDto<string[]>.SuccessResult(perfis));
        }
    }
}
