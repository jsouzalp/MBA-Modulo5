using Conteudo.Application.Commands.AtualizarCategoria;
using Conteudo.Application.Commands.CadastrarCategoria;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using Core.Services.Controllers;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Conteudo.API.Controllers;

/// <summary>
/// Controller de Categorias
/// </summary>
[ExcludeFromCodeCoverage]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CategoriaController(IMediatorHandler mediator
                              , ICategoriaAppService categoriaAppService
                              , INotificationHandler<DomainNotificacaoRaiz> notifications
                              , INotificador notificador) : MainController(mediator, notifications, notificador)
{
    /// <summary>
    /// Retorna uma categoria pelo ID.
    /// </summary>
    /// <param name="id">ID do curso</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseResult<CategoriaDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var categoria = await categoriaAppService.ObterPorIdAsync(id);

        if (categoria == null)
        {
            _notificador.AdicionarErro("Categoria não encontrada.");
            return RespostaPadraoApi<string>();
        }

        return RespostaPadraoApi(data: categoria);
    }

    /// <summary>
    /// Retorna todas as categorias.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoriaDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterTodos()
    {
        var categorias = await categoriaAppService.ObterTodasCategoriasAsync();
        return RespostaPadraoApi(data: categorias);
    }

    /// <summary>
    /// Cadastra uma nova categoria.
    /// </summary>
    /// <param name="dto">Dados da categoria</param>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseResult<Guid?>), 201)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CadastrarCategoria([FromBody] CadastroCategoriaDto dto)
    {
        var command = dto.Adapt<CadastrarCategoriaCommand>();
        return RespostaPadraoApi(HttpStatusCode.Created, await _mediatorHandler.ExecutarComando(command));
    }

    /// <summary>
    /// Atualiza uma categoria existente.
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <param name="dto">Dados da categoria</param>
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ResponseResult<bool?>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> AtualizarCategoria(Guid id, [FromBody] AtualizarCategoriaDto dto)
    {
        if (id != dto.Id)
            return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID da categoria não confere.");

        var command = dto.Adapt<AtualizarCategoriaCommand>();
        return RespostaPadraoApi<bool?>(await _mediatorHandler.ExecutarComando(command));
    }
}
