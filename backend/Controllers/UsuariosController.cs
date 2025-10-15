using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PimApi.DTOs;
using PimApi.Services;

namespace PimApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponseDto<UsuarioResponseDto>>> CadastrarUsuario([FromBody] CadastroUsuarioRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseDto<UsuarioResponseDto>.ErrorResult("Dados inválidos"));
            }

            try
            {
                var result = await _usuarioService.CadastrarAsync(request);
                return CreatedAtAction(nameof(ObterUsuario), new { id = result.Id }, 
                    ApiResponseDto<UsuarioResponseDto>.SuccessResult(result, "Usuário cadastrado com sucesso"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDto<UsuarioResponseDto>.ErrorResult(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<UsuarioResponseDto>.ErrorResult("Erro interno do servidor"));
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<UsuarioResponseDto>>>> ListarUsuarios(
            [FromQuery] string? status = null,
            [FromQuery] string? perfilAcesso = null,
            [FromQuery] string? busca = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _usuarioService.ListarAsync(status, perfilAcesso, busca, page, pageSize);
                return Ok(ApiResponseDto<List<UsuarioResponseDto>>.SuccessResult(result));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<List<UsuarioResponseDto>>.ErrorResult("Erro interno do servidor"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<UsuarioResponseDto>>> ObterUsuario(int id)
        {
            try
            {
                var result = await _usuarioService.ObterPorIdAsync(id);
                
                if (result == null)
                {
                    return NotFound(ApiResponseDto<UsuarioResponseDto>.ErrorResult("Usuário não encontrado"));
                }

                return Ok(ApiResponseDto<UsuarioResponseDto>.SuccessResult(result));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<UsuarioResponseDto>.ErrorResult("Erro interno do servidor"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponseDto<UsuarioResponseDto>>> EditarUsuario(int id, [FromBody] EditarUsuarioRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseDto<UsuarioResponseDto>.ErrorResult("Dados inválidos"));
            }

            try
            {
                var result = await _usuarioService.EditarAsync(id, request);
                return Ok(ApiResponseDto<UsuarioResponseDto>.SuccessResult(result, "Usuário editado com sucesso"));
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Usuário não encontrado")
                {
                    return NotFound(ApiResponseDto<UsuarioResponseDto>.ErrorResult(ex.Message));
                }
                return BadRequest(ApiResponseDto<UsuarioResponseDto>.ErrorResult(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<UsuarioResponseDto>.ErrorResult("Erro interno do servidor"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponseDto<object>>> DesativarUsuario(int id)
        {
            try
            {
                var result = await _usuarioService.DesativarAsync(id);
                
                if (!result)
                {
                    return NotFound(ApiResponseDto<object>.ErrorResult("Usuário não encontrado"));
                }

                return Ok(ApiResponseDto<object>.SuccessResult(null, "Usuário desativado com sucesso"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<object>.ErrorResult("Erro interno do servidor"));
            }
        }

        [HttpPost("{id}/reset-senha")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponseDto<object>>> ResetarSenha(int id)
        {
            try
            {
                var result = await _usuarioService.ResetarSenhaAsync(id);
                
                if (!result)
                {
                    return NotFound(ApiResponseDto<object>.ErrorResult("Usuário não encontrado"));
                }

                return Ok(ApiResponseDto<object>.SuccessResult(null, "Senha resetada para padrão"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<object>.ErrorResult("Erro interno do servidor"));
            }
        }
    }
}
