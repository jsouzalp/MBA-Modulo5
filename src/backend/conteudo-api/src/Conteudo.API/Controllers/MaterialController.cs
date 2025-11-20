using Conteudo.Application.Commands.AtualizarMaterial;
using Conteudo.Application.Commands.CadastrarMaterial;
using Conteudo.Application.Commands.ExcluirMaterial;
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
/// Controller para gerenciar materiais
/// </summary>
[ExcludeFromCodeCoverage]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class MaterialController(IMediatorHandler mediator,
                                IMaterialAppService materialAppService,
                                INotificationHandler<DomainNotificacaoRaiz> notifications,
                                INotificador notificador) : MainController(mediator, notifications, notificador)
{
    /// <summary>
    /// Obtém um material por ID
    /// </summary>
    /// <param name="id">ID do material</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseResult<MaterialDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var material = await materialAppService.ObterPorIdAsync(id);

            if (material == null)
            {
                _notificador.AdicionarErro("Material não encontrado.");
                return RespostaPadraoApi<string>();
            }

            return RespostaPadraoApi(data: material);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Obtém materiais por aula
    /// </summary>
    /// <param name="aulaId">ID da aula</param>
    [HttpGet("aula/{aulaId}")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<MaterialDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterPorAulaId(Guid aulaId)
    {
        try
        {
            if (aulaId == Guid.Empty)
                return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID da aula inválido");

            var materiais = await materialAppService.ObterPorAulaIdAsync(aulaId);
            return RespostaPadraoApi(data: materiais);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Obtém materiais ativos
    /// </summary>
    [HttpGet("ativos")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<MaterialDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterAtivos()
    {
        try
        {
            var materiais = await materialAppService.ObterAtivosAsync();
            return RespostaPadraoApi(data: materiais);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Obtém materiais ativos por aula
    /// </summary>
    /// <param name="aulaId">ID da aula</param>
    [HttpGet("aula/{aulaId}/ativos")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<MaterialDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterAtivosPorAulaId(Guid aulaId)
    {
        try
        {
            if (aulaId == Guid.Empty)
                return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID da aula inválido");

            var materiais = await materialAppService.ObterAtivosPorAulaIdAsync(aulaId);
            return RespostaPadraoApi(data: materiais);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Obtém materiais obrigatórios por aula
    /// </summary>
    /// <param name="aulaId">ID da aula</param>
    [HttpGet("aula/{aulaId}/obrigatorios")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<MaterialDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterObrigatoriosPorAulaId(Guid aulaId)
    {
        try
        {
            if (aulaId == Guid.Empty)
                return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID da aula inválido");

            var materiais = await materialAppService.ObterObrigatoriosPorAulaIdAsync(aulaId);
            return RespostaPadraoApi(data: materiais);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Cadastra um novo material
    /// </summary>
    /// <param name="dto">Dados do material</param>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseResult<Guid?>), 201)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Cadastrar([FromBody] CadastroMaterialDto dto)
    {
        try
        {
            var command = dto.Adapt<CadastrarMaterialCommand>();
            return RespostaPadraoApi(HttpStatusCode.Created, await _mediatorHandler.ExecutarComando(command));
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Atualiza um material existente
    /// </summary>
    /// <param name="id">ID do material</param>
    /// <param name="dto">Dados atualizados do material</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseResult<bool?>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarMaterialDto dto)
    {
        try
        {
            if (id != dto.Id)
                return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do material não confere");

            var command = dto.Adapt<AtualizarMaterialCommand>();
            return RespostaPadraoApi<bool?>(await _mediatorHandler.ExecutarComando(command));
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Exclui um material
    /// </summary>
    /// <param name="id">ID do material</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseResult<bool?>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        try
        {
            var command = new ExcluirMaterialCommand(id);
            return RespostaPadraoApi<bool?>(await _mediatorHandler.ExecutarComando(command));
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }
}
