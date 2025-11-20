using Core.Mediator;
using Core.Messages;
using Core.Notification;
using Core.Services.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Controllers;

/// <summary>
/// Controller base para o BFF que herda do MainController para padronizar respostas
/// </summary>
[ExcludeFromCodeCoverage]
[ApiController]
public abstract class BffController : MainController
{
    protected BffController(IMediatorHandler mediator, INotificationHandler<DomainNotificacaoRaiz> notifications, INotificador notificador) : base(mediator, notifications, notificador)
    { }

    protected async Task<ActionResult> ProcessarRespostaApi(HttpResponseMessage response, string? successMessage = null)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            try
            {
                // Tenta deserializar como ResponseResult (formato das APIs)
                var responseResult = System.Text.Json.JsonSerializer.Deserialize<Core.Communication.ResponseResult<object>>(
                    content,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (responseResult != null)
                {
                    // Se for um ResponseResult, retorna os dados no formato padronizado
                    return RespostaPadraoApi<object>(
                        (System.Net.HttpStatusCode)responseResult.Status,
                        responseResult.Data,
                        successMessage ?? responseResult.Title
                    );
                }
                else
                {
                    // Se não for ResponseResult, retorna o conteúdo direto
                    return RespostaPadraoApi<object>(
                        response.StatusCode,
                        content,
                        successMessage ?? "Operação realizada com sucesso"
                    );
                }
            }
            catch
            {
                // Se falhar na deserialização, retorna o conteúdo direto
                return RespostaPadraoApi<object>(
                    response.StatusCode,
                    content,
                    successMessage ?? "Operação realizada com sucesso"
                );
            }
        }
        else
        {
            try
            {
                var responseResult = System.Text.Json.JsonSerializer.Deserialize<Core.Communication.ResponseResult<object>>(
                    content,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (responseResult != null && responseResult.Errors?.Mensagens?.Any() == true)
                {
                    foreach (var erro in responseResult.Errors.Mensagens)
                    {
                        _notificador.AdicionarErro(erro);
                    }
                }
                else
                {
                    _notificador.AdicionarErro($"Erro na API externa: {response.StatusCode} - {content}");
                }
            }
            catch
            {
                _notificador.AdicionarErro($"Erro na API externa: {response.StatusCode} - {content}");
            }

            return RespostaPadraoApi<object>(
                response.StatusCode,
                message: $"Erro na comunicação com a API externa: {response.StatusCode}"
            );
        }
    }

    protected ActionResult ProcessarErro(System.Net.HttpStatusCode statusCode, string message)
    {
        _notificador.AdicionarErro(message);
        return RespostaPadraoApi<object>(statusCode, message: message);
    }
}
