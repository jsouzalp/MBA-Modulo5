using Auth.API.Models.Requests;
using Auth.Application.Services;
using Auth.Domain.Entities;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Messages.Integration;
using Core.Notification;
using Core.Services.Controllers;
using FluentValidation.Results;
using MediatR;
using MessageBus;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Auth.API.Controllers;

/// <summary>
/// Controller de autenticação
/// </summary>
[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController(IMediatorHandler mediator
                                  , AuthService authService
                                  , INotificationHandler<DomainNotificacaoRaiz> notifications, IMessageBus bus, INotificador notificador) : MainController(mediator, notifications, notificador)
{

    /// <summary>
    /// Registra um novo usuário no sistema
    /// </summary>
    /// <param name="registroRequest">Dados do usuário para registro</param>
    /// <returns>Confirmação de registro</returns>
    [HttpPost("registro")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ApiError), 400)]
    public async Task<IActionResult> Registro([FromBody] RegistroRequest registroRequest)
    {
        if (!ModelState.IsValid) return RespostaPadraoApi(HttpStatusCode.BadRequest, ModelState);

        var user = new ApplicationUser
        {
            Nome = registroRequest.Nome,
            UserName = registroRequest.Email,
            Email = registroRequest.Email,
            EmailConfirmed = true
        };

        var result = await authService.UserManager.CreateAsync(user, registroRequest.Senha);

        if (result.Succeeded)
        {
            var roleName = registroRequest.EhAdministrador ? "Administrador" : "Usuario";
            await authService.UserManager.AddToRoleAsync(user, roleName);

            if (!registroRequest.EhAdministrador)
            {
                var clienteResult = await RegistrarAluno(registroRequest);

                if (!clienteResult.ValidationResult.IsValid)
                {
                    await authService.UserManager.DeleteAsync(user);
                    return RespostaPadraoApi(HttpStatusCode.BadRequest, clienteResult.ValidationResult);
                }
            }

            return RespostaPadraoApi(HttpStatusCode.OK, await authService.GerarJwt(registroRequest.Email));
        }

        foreach (var error in result.Errors)
        {
            _notificador.AdicionarErro(error.Description);
        }

        return RespostaPadraoApi(HttpStatusCode.BadRequest, string.Empty);
    }

    /// <summary>
    /// Autentica um usuário no sistema
    /// </summary>
    /// <param name="loginRequest">Credenciais do usuário</param>
    /// <returns>Token de autenticação</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ApiError), 401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid) return RespostaPadraoApi(HttpStatusCode.BadRequest, ModelState);

        var result = await authService.SignInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Senha, false, true);

        if (result.Succeeded)
        {
            return RespostaPadraoApi(HttpStatusCode.OK, await authService.GerarJwt(loginRequest.Email));
        }

        if (result.IsLockedOut)
        {
            _notificador.AdicionarErro("Usuário temporariamente bloqueado por tentativas inválidas");
            return RespostaPadraoApi(HttpStatusCode.Forbidden, "Usuário temporariamente bloqueado por tentativas inválidas");
        }

        _notificador.AdicionarErro("Usuário ou Senha incorretos");
        return RespostaPadraoApi(HttpStatusCode.BadRequest, "Usuário ou Senha incorretos");
    }

    /// <summary>
    /// Renova o token de autenticação
    /// </summary>
    /// <param name="refreshToken">Token de refresh</param>
    /// <returns>Novo token de autenticação</returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ApiError), 401)]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            _notificador.AdicionarErro("Refresh Token inválido.");
            return RespostaPadraoApi(HttpStatusCode.BadRequest, "Refresh Token inválido.");
        }

        var token = await authService.ObterRefreshToken(Guid.Parse(refreshToken));

        if (token is null)
        {
            _notificador.AdicionarErro("Refresh Token expirado");
            return RespostaPadraoApi(HttpStatusCode.BadRequest, "Refresh Token expirado");
        }

        return RespostaPadraoApi(HttpStatusCode.OK, await authService.GerarJwt(token.Username));
    }

    private async Task<ResponseMessage> RegistrarAluno(RegistroRequest registroRequest)
    {
        var usuario = await authService.UserManager.FindByEmailAsync(registroRequest.Email);

        var usuarioRegistrado = new AlunoRegistradoIntegrationEvent(
             Guid.Parse(usuario!.Id),
             registroRequest.Nome,
             registroRequest.Email,
             registroRequest.CPF,
             registroRequest.DataNascimento,
             registroRequest.Telefone,
             registroRequest.Genero,
             registroRequest.Cidade,
             registroRequest.Estado,
             registroRequest.CEP,
             registroRequest.Foto
         );

        try
        {
            return await bus.RequestAsync<AlunoRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
        }
        catch (Exception ex)
        {
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("RegistroAluno", $"Falha ao registrar aluno no serviço de Alunos: {ex.Message}"));
            return new ResponseMessage(validationResult);
        }
    }
}
