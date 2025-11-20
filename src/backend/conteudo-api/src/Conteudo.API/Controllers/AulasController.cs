using Conteudo.Application.Commands.AtualizarAula;
using Conteudo.Application.Commands.CadastrarAula;
using Conteudo.Application.Commands.DespublicarAula;
using Conteudo.Application.Commands.ExcluirAula;
using Conteudo.Application.Commands.PublicarAula;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Core.Communication;
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
/// Controller para gerenciar aulas
/// </summary>
[ExcludeFromCodeCoverage]
[Route("api/cursos/{cursoId}/[controller]")]
[Authorize]
[Produces("application/json")]
public class AulasController(IMediatorHandler mediator,
                            IAulaAppService aulaAppService,
                            INotificationHandler<DomainNotificacaoRaiz> notifications,
                            INotificador notificador) : MainController(mediator, notifications, notificador)
{
    /// <summary>
    /// Obtém uma aula por ID
    /// </summary>
    /// <param name="cursoId">ID do curso</param>
    /// <param name="id">ID da aula</param>
    /// <param name="includeMateriais">Se deve incluir materiais na resposta</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseResult<AulaDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterPorId(Guid cursoId, Guid id, [FromQuery] bool includeMateriais = false)
    {
        try
        {
            var aula = await aulaAppService.ObterPorIdAsync(cursoId, id, includeMateriais);

            if (aula == null)
            {
                _notificador.AdicionarErro("Aula não encontrada.");
                return RespostaPadraoApi<string>();
            }

            return RespostaPadraoApi(data: aula);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Obtém todas as aulas
    /// </summary>
    /// <param name="cursoId">ID do curso</param>
    /// <param name="includeMateriais">Se deve incluir materiais na resposta</param>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<AulaDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterTodos(Guid cursoId, [FromQuery] bool includeMateriais = false)
    {
        try
        {
            var aulas = await aulaAppService.ObterTodosAsync(cursoId, includeMateriais);
            return RespostaPadraoApi(data: aulas);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Obtém aulas publicadas
    /// </summary>
    /// <param name="includeMateriais">Se deve incluir materiais na resposta</param>
    [HttpGet("publicadas")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<AulaDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterPublicadas([FromQuery] bool includeMateriais = false)
    {
        try
        {
            var aulas = await aulaAppService.ObterPublicadasAsync(includeMateriais);
            return RespostaPadraoApi(data: aulas);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Cadastra uma nova aula
    /// </summary>
    /// <param name="cursoId">ID do curso ao qual a aula pertence</param>
    /// <param name="dto">Dados da aula</param>
    [HttpPost()]
    [ProducesResponseType(typeof(ResponseResult<Guid?>), 201)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Cadastrar([FromRoute] Guid cursoId, [FromBody] CadastroAulaDto dto)
    {
        try
        {
            if (cursoId != dto.CursoId)
            {
                _notificador.AdicionarErro("ID do curso informado não confere.");
                return RespostaPadraoApi<string>();
            }

            var command = dto.Adapt<CadastrarAulaCommand>();
            return RespostaPadraoApi<Guid>(await _mediatorHandler.ExecutarComando(command), HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Atualiza uma aula existente
    /// </summary>
    /// <param name="cursoId">ID do curso ao qual a aula pertence</param>
    /// <param name="id">ID da aula</param>
    /// <param name="dto">Dados atualizados da aula</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseResult<bool?>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Atualizar([FromRoute] Guid cursoId, Guid id, [FromBody] AtualizarAulaDto dto)
    {
        try
        {
            if (cursoId != dto.CursoId)
            {
                _notificador.AdicionarErro("ID do curso informado não confere.");
                return RespostaPadraoApi<string>();
            }
            if (id != dto.Id)
            {
                _notificador.AdicionarErro("ID da aula não confere.");
                return RespostaPadraoApi<string>();
            }

            var command = dto.Adapt<AtualizarAulaCommand>();
            return RespostaPadraoApi<bool?>(await _mediatorHandler.ExecutarComando(command));
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Exclui uma aula
    /// </summary>
    /// <param name="cursoId">ID do curso ao qual a aula pertence</param>
    /// <param name="id">ID da aula</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Excluir(Guid cursoId, Guid id)
    {
        try
        {
            var command = new ExcluirAulaCommand(cursoId, id);
            return RespostaPadraoApi<bool>(await _mediatorHandler.ExecutarComando(command));
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Publica uma aula
    /// </summary>
    /// <param name="cursoId">ID do curso ao qual a aula pertence</param>
    /// <param name="id">ID da aula</param>
    [HttpPost("{id}/publicar")]
    [ProducesResponseType(typeof(ResponseResult<bool?>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Publicar(Guid cursoId, Guid id)
    {
        try
        {
            var command = new PublicarAulaCommand(cursoId, id);
            return RespostaPadraoApi<bool?>(await _mediatorHandler.ExecutarComando(command));
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Despublica uma aula
    /// </summary>
    /// <param name="cursoId">ID do curso ao qual a aula pertence</param>
    /// <param name="id">ID da aula</param>
    [HttpPost("{id}/despublicar")]
    [ProducesResponseType(typeof(ResponseResult<bool?>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Despublicar(Guid cursoId, Guid id)
    {
        try
        {
            var command = new DespublicarAulaCommand(cursoId, id);
            return RespostaPadraoApi<bool?>(await _mediatorHandler.ExecutarComando(command));
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }
}
