using BFF.API.Models.Response;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Controllers;

/// <summary>
/// Controller de Health Check da API
/// </summary>
[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/[controller]")]
public class HealthController(
    IMediatorHandler mediator,
    INotificationHandler<DomainNotificacaoRaiz> notifications,
    INotificador notificador) : BffController(mediator, notifications, notificador)
{
    /// <summary>
    /// Verificar saúde da API
    /// </summary>
    /// <returns>Status de saúde da API</returns>
    [HttpGet]
    public IActionResult Obter()
    {
        var healthData = new HealthCheckResponse
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
            Services = new List<ServiceHealthResponse>
                {
                    new() { Name = "BFF API", Status = "Healthy", ResponseTime = "0ms", LastCheck = DateTime.UtcNow }
                }
        };

        return RespostaPadraoApi<HealthCheckResponse>(System.Net.HttpStatusCode.OK, healthData, "API funcionando normalmente");
    }

    /// <summary>
    /// Obter status detalhado da API
    /// </summary>
    /// <returns>Status detalhado da API</returns>
    [HttpGet("status")]
    public IActionResult ObterStatus()
    {
        var statusData = new ApiStatusResponse
        {
            Name = "BFF API",
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
            StartTime = DateTime.UtcNow.AddHours(-1),
            Uptime = TimeSpan.FromHours(1),
            Status = "Running",
            Configuration = new Dictionary<string, object>
                {
                    { "LogLevel", "Information" },
                    { "Caching", "Enabled" },
                    { "Compression", "Enabled" }
                }
        };

        return RespostaPadraoApi<ApiStatusResponse>(System.Net.HttpStatusCode.OK, statusData, "Status da API obtido com sucesso");
    }
}
