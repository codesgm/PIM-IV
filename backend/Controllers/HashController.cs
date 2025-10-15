using Microsoft.AspNetCore.Mvc;

namespace PimApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HashController : ControllerBase
    {
        [HttpPost("generate")]
        public ActionResult<object> GenerateHash([FromBody] string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            return Ok(new { password, hash });
        }
    }
}
