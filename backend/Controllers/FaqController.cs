using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PimApi.Data;
using PimApi.DTOs;
using PimApi.Models;

namespace PimApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaqController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FaqController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<FaqResponseDto>>>> ListarFaqs()
        {
            try
            {
                var faqs = await _context.Faqs
                    .Where(f => f.Ativo)
                    .OrderBy(f => f.Categoria)
                    .ThenBy(f => f.DataCriacao)
                    .Select(f => new FaqResponseDto
                    {
                        Id = f.Id,
                        Pergunta = f.Pergunta,
                        Resposta = f.Resposta,
                        Categoria = f.Categoria,
                        CategoriaNome = f.Categoria.ToString(),
                        Ativo = f.Ativo,
                        DataCriacao = f.DataCriacao,
                        DataAtualizacao = f.DataAtualizacao
                    })
                    .ToListAsync();

                return Ok(ApiResponseDto<List<FaqResponseDto>>.SuccessResult(faqs));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<List<FaqResponseDto>>.ErrorResult("Erro interno do servidor"));
            }
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponseDto<List<FaqResponseDto>>>> ListarFaqsAdmin()
        {
            try
            {
                var faqs = await _context.Faqs
                    .OrderBy(f => f.Categoria)
                    .ThenBy(f => f.DataCriacao)
                    .Select(f => new FaqResponseDto
                    {
                        Id = f.Id,
                        Pergunta = f.Pergunta,
                        Resposta = f.Resposta,
                        Categoria = f.Categoria,
                        CategoriaNome = f.Categoria.ToString(),
                        Ativo = f.Ativo,
                        DataCriacao = f.DataCriacao,
                        DataAtualizacao = f.DataAtualizacao
                    })
                    .ToListAsync();

                return Ok(ApiResponseDto<List<FaqResponseDto>>.SuccessResult(faqs));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<List<FaqResponseDto>>.ErrorResult("Erro interno do servidor"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<FaqResponseDto>>> ObterFaq(int id)
        {
            try
            {
                var faq = await _context.Faqs.FindAsync(id);
                
                if (faq == null)
                {
                    return NotFound(ApiResponseDto<FaqResponseDto>.ErrorResult("FAQ não encontrado"));
                }

                var faqDto = new FaqResponseDto
                {
                    Id = faq.Id,
                    Pergunta = faq.Pergunta,
                    Resposta = faq.Resposta,
                    Categoria = faq.Categoria,
                    CategoriaNome = faq.Categoria.ToString(),
                    Ativo = faq.Ativo,
                    DataCriacao = faq.DataCriacao,
                    DataAtualizacao = faq.DataAtualizacao
                };

                return Ok(ApiResponseDto<FaqResponseDto>.SuccessResult(faqDto));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<FaqResponseDto>.ErrorResult("Erro interno do servidor"));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponseDto<FaqResponseDto>>> CriarFaq([FromBody] CriarFaqRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseDto<FaqResponseDto>.ErrorResult("Dados inválidos"));
            }

            try
            {
                var faq = new Faq
                {
                    Pergunta = request.Pergunta,
                    Resposta = request.Resposta,
                    Categoria = request.Categoria,
                    DataCriacao = DateTime.UtcNow
                };

                _context.Faqs.Add(faq);
                await _context.SaveChangesAsync();

                var faqDto = new FaqResponseDto
                {
                    Id = faq.Id,
                    Pergunta = faq.Pergunta,
                    Resposta = faq.Resposta,
                    Categoria = faq.Categoria,
                    CategoriaNome = faq.Categoria.ToString(),
                    Ativo = faq.Ativo,
                    DataCriacao = faq.DataCriacao,
                    DataAtualizacao = faq.DataAtualizacao
                };

                return Ok(ApiResponseDto<FaqResponseDto>.SuccessResult(faqDto, "FAQ criado com sucesso"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<FaqResponseDto>.ErrorResult("Erro interno do servidor"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponseDto<FaqResponseDto>>> EditarFaq(int id, [FromBody] EditarFaqRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseDto<FaqResponseDto>.ErrorResult("Dados inválidos"));
            }

            try
            {
                var faq = await _context.Faqs.FindAsync(id);
                
                if (faq == null)
                {
                    return NotFound(ApiResponseDto<FaqResponseDto>.ErrorResult("FAQ não encontrado"));
                }

                faq.Pergunta = request.Pergunta;
                faq.Resposta = request.Resposta;
                faq.Categoria = request.Categoria;
                faq.Ativo = request.Ativo;
                faq.DataAtualizacao = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var faqDto = new FaqResponseDto
                {
                    Id = faq.Id,
                    Pergunta = faq.Pergunta,
                    Resposta = faq.Resposta,
                    Categoria = faq.Categoria,
                    CategoriaNome = faq.Categoria.ToString(),
                    Ativo = faq.Ativo,
                    DataCriacao = faq.DataCriacao,
                    DataAtualizacao = faq.DataAtualizacao
                };

                return Ok(ApiResponseDto<FaqResponseDto>.SuccessResult(faqDto, "FAQ editado com sucesso"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<FaqResponseDto>.ErrorResult("Erro interno do servidor"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponseDto<object>>> ExcluirFaq(int id)
        {
            try
            {
                var faq = await _context.Faqs.FindAsync(id);
                
                if (faq == null)
                {
                    return NotFound(ApiResponseDto<object>.ErrorResult("FAQ não encontrado"));
                }

                _context.Faqs.Remove(faq);
                await _context.SaveChangesAsync();

                return Ok(ApiResponseDto<object>.SuccessResult(null, "FAQ excluído com sucesso"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<object>.ErrorResult("Erro interno do servidor"));
            }
        }

        [HttpGet("categoria/{categoria}")]
        public async Task<ActionResult<ApiResponseDto<List<FaqResponseDto>>>> ListarFaqsPorCategoria(CategoriaFaq categoria)
        {
            try
            {
                var faqs = await _context.Faqs
                    .Where(f => f.Ativo && f.Categoria == categoria)
                    .OrderBy(f => f.DataCriacao)
                    .Select(f => new FaqResponseDto
                    {
                        Id = f.Id,
                        Pergunta = f.Pergunta,
                        Resposta = f.Resposta,
                        Categoria = f.Categoria,
                        CategoriaNome = f.Categoria.ToString(),
                        Ativo = f.Ativo,
                        DataCriacao = f.DataCriacao,
                        DataAtualizacao = f.DataAtualizacao
                    })
                    .ToListAsync();

                return Ok(ApiResponseDto<List<FaqResponseDto>>.SuccessResult(faqs));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDto<List<FaqResponseDto>>.ErrorResult($"Erro interno: {ex.Message}"));
            }
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<ApiResponseDto<List<FaqResponseDto>>>> BuscarFaqs([FromQuery] string termo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(termo))
                {
                    return await ListarFaqs();
                }

                var faqs = await _context.Faqs
                    .Where(f => f.Ativo && f.Pergunta.Contains(termo))
                    .OrderBy(f => f.Categoria)
                    .ThenBy(f => f.DataCriacao)
                    .Select(f => new FaqResponseDto
                    {
                        Id = f.Id,
                        Pergunta = f.Pergunta,
                        Resposta = f.Resposta,
                        Categoria = f.Categoria,
                        CategoriaNome = f.Categoria.ToString(),
                        Ativo = f.Ativo,
                        DataCriacao = f.DataCriacao,
                        DataAtualizacao = f.DataAtualizacao
                    })
                    .ToListAsync();

                return Ok(ApiResponseDto<List<FaqResponseDto>>.SuccessResult(faqs));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponseDto<List<FaqResponseDto>>.ErrorResult("Erro interno do servidor"));
            }
        }

        [HttpGet("export")]
        public async Task<ActionResult> ExportFaqs()
        {
            try
            {
                var faqs = await _context.Faqs
                    .Where(f => f.Ativo)
                    .Select(f => new {
                        f.Pergunta,
                        f.Resposta,
                        f.Categoria
                    })
                    .ToListAsync();

                return Ok(faqs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao exportar FAQs");
            }
        }
    }
}
