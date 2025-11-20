using BFF.API.Models.Request;
using BFF.API.Services.Conteudos;
using BFF.Domain.DTOs;
using Core.Communication;
using Core.Communication.Filters;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace BFF.API.Controllers;

/// <summary>
/// Controller de Conteudos no BFF - Orquestra chamadas para Conteudo.API
/// </summary>
[ExcludeFromCodeCoverage]
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ConteudosController(IMediatorHandler mediator,
                         INotificationHandler<DomainNotificacaoRaiz> notifications,
                         INotificador notificador,
                         IConteudoService conteudoService) : BffController(mediator, notifications, notificador)
{
    /// <summary>
    /// Obtém um curso por ID
    /// </summary>
    /// <param name="cursoId">ID do curso</param>
    /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
    /// <returns>Dados do curso</returns>
    [HttpGet("{cursoId}")]
    [ProducesResponseType(typeof(ResponseResult<CursoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterCurso([FromRoute] Guid cursoId, [FromQuery] bool includeAulas = false)
    {
        if (cursoId == Guid.Empty)
        {
            return ProcessarErro(HttpStatusCode.BadRequest, "Id do curso inválido.");
        }

        var resultado = await conteudoService.ObterCursoPorIdAsync(cursoId, includeAulas);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Obter todos os cursos
    /// </summary>
    /// <param name="filter">Filtro para paginação e busca</param>
    [HttpGet("cursos")]
    [ProducesResponseType(typeof(ResponseResult<PagedResult<CursoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterTodosCursos([FromQuery] CursoFilter filter)
    {
        var resultado = await conteudoService.ObterTodosCursosAsync(filter);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }
        return BadRequest(resultado);
    }

    /// <summary>
    /// Obtém cursos por categoria
    /// </summary>
    /// <param name="categoriaId">ID da categoria</param>
    /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
    /// <returns>Lista de cursos da categoria</returns>
    [HttpGet("categoria/{categoriaId}")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<CursoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterCursosPorCategoria([FromRoute] Guid categoriaId, [FromQuery] bool includeAulas = false)
    {
        var resultado = await conteudoService.ObterPorCategoriaIdAsync(categoriaId, includeAulas);

        if (resultado?.Status == (int)HttpStatusCode.OK)
            return Ok(resultado);

        return BadRequest(resultado);
    }

    /// <summary>
    /// Obtém todas as categorias
    /// </summary>
    /// <returns>Lista categorias</returns>
    [HttpGet("categorias")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<CategoriaDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterTodasCategorias()
    {
        var resultado = await conteudoService.ObterTodasCategoriasAsync();

        if (resultado?.Status == (int)HttpStatusCode.OK)
            return Ok(resultado);

        return BadRequest(resultado);
    }

    /// <summary>
    /// Cadastrar um novo curso
    /// </summary>
    [HttpPost("cursos")]
    [ProducesResponseType(typeof(ResponseResult<Guid?>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AdicionarCurso([FromBody] CursoCriarRequest curso)
    {
        var response = await conteudoService.AdicionarCursoAsync(curso);

        if (response?.Status == (int)HttpStatusCode.BadRequest)
            return BadRequest(response);

        return StatusCode(response?.Status ?? 500, response);
    }

    /// <summary>
    /// Atualizar um curso existente
    /// </summary>
    [HttpPut("cursos/{cursoId}")]
    [ProducesResponseType(typeof(ResponseResult<CursoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AtualizarCurso(Guid cursoId, [FromBody] AtualizarCursoRequest curso)
    {
        var response = await conteudoService.AtualizarCursoAsync(cursoId, curso);

        if (response?.Status == (int)HttpStatusCode.BadRequest)
            return BadRequest(response);

        return Ok(response);
    }

    /// <summary>
    /// Excluir um curso
    /// </summary>
    [HttpDelete("cursos/{cursoId}")]
    [ProducesResponseType(typeof(ResponseResult<bool?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ExcluirCurso(Guid cursoId)
    {
        var response = await conteudoService.ExcluirCursoAsync(cursoId);

        if (response?.Status == (int)HttpStatusCode.BadRequest)
            return BadRequest(response);

        return Ok(response);
    }

    /// <summary>
    /// Obter Aulas por Curso ID
    /// </summary>
    [HttpGet("cursos/{cursoId}/aulas")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<AulaDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterAulasPorCursoId([FromRoute] Guid cursoId)
    {
        var resultado = await conteudoService.ObterCursoPorIdAsync(cursoId, true);
        if (resultado?.Status == (int)HttpStatusCode.OK)
            return Ok(resultado);
        return BadRequest(resultado);
    }

    /// <summary>
    /// Obter Conteudo Programatico por Curso ID
    /// </summary>
    [HttpGet("cursos/{cursoId}/conteudo-programatico")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<AulaDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterConteudoProgramaticoPorCursoId([FromRoute] Guid cursoId)
    {
        var resultado = await conteudoService.ObterConteudoProgramaticoPorCursoIdAsync(cursoId);
        if (resultado?.Status == (int)HttpStatusCode.OK)
            return Ok(resultado);
        return BadRequest(resultado);
    }

    /// <summary>
    /// Cadastrar uma nova aula em um curso
    /// </summary>
    [HttpPost("cursos/{cursoId}/aulas")]
    [ProducesResponseType(typeof(ResponseResult<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AdicionarAula([FromRoute] Guid cursoId,
                                                    [FromBody] AulaCriarRequest request)
    {
        var response = await conteudoService.AdicionarAulaAsync(cursoId, request);

        if (response?.Status == (int)HttpStatusCode.BadRequest)
            return BadRequest(response);

        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Atualizar uma aula existente
    /// </summary>
    [HttpPut("cursos/{cursoId}/aulas/{aulaId}")]
    [ProducesResponseType(typeof(ResponseResult<AulaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AtualizarAula(
        [FromRoute] Guid cursoId,
        [FromRoute] Guid aulaId,
        [FromBody] AulaAtualizarRequest request)
    {
        if (aulaId != request.Id)
            return ProcessarErro(HttpStatusCode.BadRequest, "ID da aula na rota não confere com o corpo.");

        var response = await conteudoService.AtualizarAulaAsync(cursoId, request);

        if (response?.Status == (int)HttpStatusCode.BadRequest)
            return BadRequest(response);

        return Ok(response);
    }

    /// <summary>
    /// Excluir uma aula
    /// </summary>
    [HttpDelete("cursos/{cursoId}/aulas/{aulaId}")]
    [ProducesResponseType(typeof(ResponseResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ExcluirAula(
        [FromRoute] Guid cursoId,
        [FromRoute] Guid aulaId)
    {
        var response = await conteudoService.ExcluirAulaAsync(cursoId, aulaId);

        if (response?.Status == (int)HttpStatusCode.BadRequest)
            return BadRequest(response);

        return Ok(response);
    }
}
