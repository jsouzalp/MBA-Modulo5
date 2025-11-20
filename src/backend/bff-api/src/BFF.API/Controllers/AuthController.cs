using BFF.API.Models.Request;
using BFF.API.Models.Response;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Controllers;

/// <summary>
/// Controller de autenticação no BFF - Proxy para Auth API
/// </summary>
[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IApiClientService apiClient,
    IOptions<ApiSettings> apiSettings,
    ILogger<AuthController> logger,
    IMediatorHandler mediator,
    INotificationHandler<DomainNotificacaoRaiz> notifications,
    INotificador notificador) : BffController(mediator, notifications, notificador)
{
    private readonly ApiSettings _apiSettings = apiSettings.Value;

    /// <summary>
    /// Registrar novo usuário
    /// </summary>
    /// <param name="request">Dados de registro</param>
    /// <returns>Resposta da autenticação</returns>
    [HttpPost("registro")]
    [ProducesResponseType(typeof(ResponseResult<AuthRegistroResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Registro([FromBody] RegistroRequest request)
    {
        try
        {
            apiClient.SetBaseAddress(_apiSettings.AuthApiUrl);
            var result = await apiClient.PostAsyncWithActionResult<RegistroRequest, ResponseResult<AuthRegistroResponse>>(
                "/api/auth/registro",
                request,
                "Usuário registrado com sucesso");

            if (result.Success && result.Data != null)
            {
                var dataType = result.Data.GetType();
                if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(ResponseResult<>))
                {
                    var dataProperty = dataType.GetProperty("Data");
                    var innerData = dataProperty?.GetValue(result.Data);
                    if (innerData != null)
                    {
                        return RespostaPadraoApi(System.Net.HttpStatusCode.OK, innerData, result.Message);
                    }
                }

                return RespostaPadraoApi(System.Net.HttpStatusCode.OK, result.Data, result.Message);
            }

            if (result.ErrorContent != null)
            {
                return StatusCode(result.StatusCode, result.ErrorContent);
            }

            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, result.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar registro via BFF para: {Email}", request.Email);
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Fazer login
    /// </summary>
    /// <param name="request">Credenciais de login</param>
    /// <returns>Resposta da autenticação</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseResult<AuthLoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            apiClient.SetBaseAddress(_apiSettings.AuthApiUrl);
            var result = await apiClient.PostAsyncWithActionResult<LoginRequest, ResponseResult<AuthLoginResponse>>(
                "/api/auth/login",
                request,
                "Login realizado com sucesso");

            if (result.Success && result.Data != null)
            {
                var dataType = result.Data.GetType();
                if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(ResponseResult<>))
                {
                    var dataProperty = dataType.GetProperty("Data");
                    var innerData = dataProperty?.GetValue(result.Data);
                    if (innerData != null)
                    {
                        return RespostaPadraoApi(System.Net.HttpStatusCode.OK, innerData, result.Message);
                    }
                }

                return RespostaPadraoApi(System.Net.HttpStatusCode.OK, result.Data, result.Message);
            }

            if (result.ErrorContent != null)
            {
                return StatusCode(result.StatusCode, result.ErrorContent);
            }

            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, result.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar login via BFF para: {Email}", request.Email);
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Renovar token
    /// </summary>
    /// <param name="request">Request de refresh token</param>
    /// <returns>Novo token</returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseResult<AuthRefreshTokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            apiClient.SetBaseAddress(_apiSettings.AuthApiUrl);
            var result = await apiClient.PostAsyncWithActionResult<RefreshTokenRequest, ResponseResult<AuthRefreshTokenResponse>>("/api/auth/refresh-token", request, "Token renovado com sucesso");

            if (result.Success && result.Data != null)
            {
                var dataType = result.Data.GetType();
                if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(ResponseResult<>))
                {
                    var dataProperty = dataType.GetProperty("Data");
                    var innerData = dataProperty?.GetValue(result.Data);
                    if (innerData != null)
                    {
                        return RespostaPadraoApi(System.Net.HttpStatusCode.OK, innerData, result.Message);
                    }
                }

                return RespostaPadraoApi(System.Net.HttpStatusCode.OK, result.Data, result.Message);
            }

            if (result.ErrorContent != null)
            {
                return StatusCode(result.StatusCode, result.ErrorContent);
            }

            return ProcessarErro(System.Net.HttpStatusCode.BadRequest, result.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar refresh token via BFF");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }
}
