using BFF.API.Models.Request;
using BFF.API.Services.Conteudos;
using BFF.API.Services.Pagamentos;
using BFF.Domain.DTOs.Pagamentos.Response;
using Core.Communication;
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
/// Controller de Pagamentos no BFF - Orquestra chamadas para Pagamento API
/// </summary>
[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PagamentosController(IMediatorHandler mediator,
                            INotificationHandler<DomainNotificacaoRaiz> notifications,
                            INotificador notificador,
                            ILogger<PagamentosController> logger,
                            IPagamentoService pagamentoService,
                            IConteudoService conteudoService) : BffController(mediator, notifications, notificador)
{
    /// <summary>
    /// Registra um novo pagamento de curso.
    /// </summary>
    /// <param name="pagamento">Dados do pagamento a ser processado</param>
    /// <returns>
    /// Retorna o resultado do processamento do pagamento.
    /// </returns>
    /// <response code="200">Pagamento processado com sucesso</response>
    /// <response code="400">Erro de validação ou valor divergente do curso</response>
    /// <response code="500">Erro interno ao processar o pagamento</response>
    [Authorize(Roles = "Usuario, Administrador")]
    [HttpPost("registrar-pagamento")]
    [ProducesResponseType(typeof(ResponseResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Pagamento([FromBody] PagamentoCursoInputModel pagamento)
    {
        if (!ModelState.IsValid)
            return RespostaPadraoApi<CommandResult>(ModelState);

        try
        {
            var cursoResp = await conteudoService.ObterCursoPorIdAsync(pagamento.CursoId, false);

            if (cursoResp?.Status == (int)HttpStatusCode.OK)
            {
                var valorCurso = cursoResp.Data?.Valor ?? 0m;

                if (valorCurso != pagamento.Total)
                    return RespostaPadraoApi(HttpStatusCode.BadRequest, "Valor do Pagamento diverge do Valor do Curso");
            }
            else
            {
                return RespostaPadraoApi(HttpStatusCode.NotFound, "Curso não encontrado.");
            }

            var resultado = await pagamentoService.ExecutarPagamento(pagamento);

            if (resultado?.Status == (int)HttpStatusCode.OK)
            {
                return Ok(resultado);
            }

            return BadRequest(resultado);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar pagamento via BFF");
            return ProcessarErro(HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Obtém todos os pagamentos cadastrados.
    /// </summary>
    /// <returns>
    /// Lista de pagamentos disponíveis no sistema.
    /// </returns>
    /// <response code="200">Lista retornada com sucesso</response>
    /// <response code="500">Erro interno ao obter os pagamentos</response>
    [Authorize(Roles = "Usuario, Administrador")]
    [HttpGet("obter_todos")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<PagamentoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ObterTodos()
    {
        try
        {
            var pagamentos = await pagamentoService.ObterTodos();
            return RespostaPadraoApi(HttpStatusCode.OK, pagamentos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter pagamentos via BFF");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro ao obter pagamentos via BFF");
        }
    }

    /// <summary>
    /// Obtém um pagamento específico pelo seu identificador único.
    /// </summary>
    /// <param name="id">ID do pagamento (Guid)</param>
    /// <returns>
    /// Dados do pagamento solicitado.
    /// </returns>
    /// <response code="200">Pagamento encontrado</response>
    /// <response code="404">Pagamento não encontrado</response>
    /// <response code="500">Erro interno ao obter o pagamento</response>
    [Authorize(Roles = "Administrador")]
    [HttpGet("obter/{id:guid}")]
    [ProducesResponseType(typeof(ResponseResult<PagamentoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var pagamentos = await pagamentoService.ObterPorIdPagamento(id);
            return RespostaPadraoApi(HttpStatusCode.OK, pagamentos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter pagamentos via BFF");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro ao obter pagamentos via BFF");
        }
    }
}
