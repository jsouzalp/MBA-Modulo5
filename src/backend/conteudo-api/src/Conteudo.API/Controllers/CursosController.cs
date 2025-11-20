using Conteudo.Application.Commands.AtualizarCurso;
using Conteudo.Application.Commands.CadastrarCurso;
using Conteudo.Application.Commands.ExcluirCurso;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Core.Communication;
using Core.Communication.Filters;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using Core.Services.Controllers;
using Core.SharedDtos.Conteudo;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Conteudo.API.Controllers;

/// <summary>
/// Controller para gerenciar cursos
/// </summary>
[ExcludeFromCodeCoverage]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CursosController(ICursoQuery cursoAppService
                           , IMediatorHandler mediator
                           , INotificador notificador
                           , INotificationHandler<DomainNotificacaoRaiz> notifications) : MainController(mediator, notifications, notificador)
{
    /// <summary>
    /// Obtém um curso por ID
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
    /// <returns>Dados do curso</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseResult<CursoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterCurso([FromRoute] Guid id, [FromQuery] bool includeAulas = false)
    {
        var curso = await cursoAppService.ObterPorIdAsync(id, includeAulas);
        if (curso == null)
        {
            _notificador.AdicionarErro("Curso não encontrado.");
            return RespostaPadraoApi<string>();
        }

        return RespostaPadraoApi(data: curso);
    }

    /// <summary>
    /// Obtém todos os cursos
    /// </summary>
    /// <param name="filter">Filtro para paginação e busca</param>
    /// <returns>Lista de cursos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseResult<PagedResult<CursoDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterCursos([FromQuery] CursoFilter filter)
    {
        var cursos = await cursoAppService.ObterTodosAsync(filter);
        return RespostaPadraoApi(data: cursos);
    }

    /// <summary>
    /// Obtém cursos por categoria
    /// </summary>
    /// <param name="categoriaId">ID da categoria</param>
    /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
    /// <returns>Lista de cursos da categoria</returns>
    [HttpGet("categoria/{categoriaId}")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<CursoDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterCursosPorCategoria([FromRoute] Guid categoriaId, [FromQuery] bool includeAulas = false)
    {
        if (categoriaId == Guid.Empty)
            return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID da categoria inválido");

        var cursos = await cursoAppService.ObterPorCategoriaIdAsync(categoriaId, includeAulas);
        return RespostaPadraoApi(data: cursos);
    }

    /// <summary>
    /// Cadastra um novo curso
    /// </summary>
    /// <param name="dto">Dados do curso</param>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseResult<Guid?>), 201)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CadastrarCurso([FromBody] CadastroCursoDto dto)
    {
        var command = dto.Adapt<CadastrarCursoCommand>();
        return RespostaPadraoApi<Guid?>(await _mediatorHandler.ExecutarComando(command));
    }

    /// <summary>
    /// Atualiza um curso existente
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <param name="dto">Dados atualizados do curso</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseResult<CursoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AtualizarCurso([FromRoute] Guid id, [FromBody] AtualizarCursoDto dto)
    {
        if (!ModelState.IsValid)
            return RespostaPadraoApi<CommandResult>(ModelState);

        if (id != dto.Id)
            return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do curso não confere");

        var command = dto.Adapt<AtualizarCursoCommand>();

        return RespostaPadraoApi<CursoDto>(await _mediatorHandler.ExecutarComando(command));
    }

    /// <summary>
    /// Exclui um curso
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Confirmação da exclusão</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseResult<bool?>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ExcluirCurso([FromRoute] Guid id)
    {
        var command = new ExcluirCursoCommand(id);
        return RespostaPadraoApi<bool?>(await _mediatorHandler.ExecutarComando(command));
    }

    /// <summary>
    /// Obtém o conteúdo programático de um curso
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Conteúdo programático</returns>
    [HttpGet("{id}/conteudo-programatico")]
    [ProducesResponseType(typeof(ResponseResult<ConteudoProgramaticoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterConteudoProgramatico([FromRoute] Guid id)
    {
        var curso = await cursoAppService.ObterPorIdAsync(id);
        if (curso == null)
        {
            _notificador.AdicionarErro("Curso não encontrado.");
            return RespostaPadraoApi<string>();
        }

        var conteudoProgramatico = new ConteudoProgramaticoDto
        {
            Resumo = curso.Resumo,
            Descricao = curso.Descricao,
            Objetivos = curso.Objetivos,
            PreRequisitos = curso.PreRequisitos,
            PublicoAlvo = curso.PublicoAlvo,
            Metodologia = curso.Metodologia,
            Recursos = curso.Recursos,
            Avaliacao = curso.Avaliacao,
            Bibliografia = curso.Bibliografia
        };

        return RespostaPadraoApi(data: conteudoProgramatico);
    }
}
