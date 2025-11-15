using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Messages.Integration;
using Core.Notification;
using Core.Services.Controllers;
using MediatR;
using MessageBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagamentos.Application.Interfaces;
using Pagamentos.Application.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Pagamentos.API.Controllers;

[ExcludeFromCodeCoverage]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pagamentos")]
[Authorize]
public class PagamentosController(IPagamentoConsultaAppService pagamentoConsultaAppService,
                                  IMediatorHandler mediator,
                                  INotificador notificador,
                                  INotificationHandler<DomainNotificacaoRaiz> notifications,
                                  IMessageBus bus) : MainController(mediator, notifications, notificador)
{
    /// <summary>
    /// Executa o pagamento de um curso.
    /// </summary>
    /// <param name="pagamento">Dados do pagamento a ser processado</param>
    /// <returns>
    /// Resultado da operação de pagamento.
    /// </returns>
    /// <response code="200">Pagamento processado com sucesso</response>
    /// <response code="400">Erro de validação ou falha ao executar pagamento</response>
    [HttpPost("pagamento")]
    [SwaggerOperation(Summary = "Executa pagamento", Description = "Executa o pagamento do curso.")]
    [ProducesResponseType(typeof(PagamentoCursoViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Pagamento([FromBody] PagamentoCursoViewModel pagamento)
    {
        if (!ModelState.IsValid)
        {
            return RespostaPadraoApi<CommandResult>(ModelState);
        }

        var eventoPagamento = new PagamentoCursoEvent(pagamento.MatriculaId,
                                                      pagamento.AlunoId,
                                                      pagamento.Total,
                                                      pagamento.NomeCartao,
                                                      pagamento.NumeroCartao,
                                                      pagamento.ExpiracaoCartao,
                                                      pagamento.CvvCartao);

        await _mediatorHandler.PublicarEvento(eventoPagamento);

        if (OperacaoValida())
        {
            try
            {
                PagamentoMatriculaCursoIntegrationEvent pagamentoMatriculaCursoIntegrationEvent = new PagamentoMatriculaCursoIntegrationEvent(pagamento.AlunoId, pagamento.MatriculaId);
                await bus.RequestAsync<PagamentoMatriculaCursoIntegrationEvent, ResponseMessage>(pagamentoMatriculaCursoIntegrationEvent);
            }
            catch (Exception ex)
            {
                _notificador.AdicionarErro(ex.Message);
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        return RespostaPadraoApi(HttpStatusCode.OK, "");
    }

    /// <summary>
    /// Obtém todos os pagamentos cadastrados.
    /// </summary>
    /// <returns>
    /// Lista de pagamentos disponíveis no sistema.
    /// </returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [Authorize(Roles = "Administrador")]
    [HttpGet("obter_todos")]
    [SwaggerOperation(Summary = "Obtém todos os pagamentos", Description = "Retorna uma lista com todos os pagamentos.")]
    [ProducesResponseType(typeof(IEnumerable<PagamentoViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> ObterTodos()
    {
        var pagamentos = await pagamentoConsultaAppService.ObterTodos();
        return RespostaPadraoApi(HttpStatusCode.OK, pagamentos);
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
    [Authorize(Roles = "Administrador")]
    [HttpGet("obter/{id:guid}")]
    [SwaggerOperation(Summary = "Obtém pagamento por ID", Description = "Retorna os dados de um pagamento específico.")]
    [ProducesResponseType(typeof(PagamentoViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var pagamento = await pagamentoConsultaAppService.ObterPorId(id);
        if (pagamento == null)
            return NotFoundResponse("Pagamento não encontrado.");

        return RespostaPadraoApi(HttpStatusCode.OK, pagamento);
    }

    private IActionResult NotFoundResponse(string message)
    {
        _notificador.AdicionarErro(message);
        return RespostaPadraoApi(HttpStatusCode.NotFound, message);
    }
}
