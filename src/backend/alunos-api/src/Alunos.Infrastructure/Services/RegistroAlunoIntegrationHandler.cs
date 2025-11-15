using Alunos.Domain.Interfaces;
using Core.Messages;
using Core.Messages.Integration;
using MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Alunos.Infrastructure.Services;

[ExcludeFromCodeCoverage]
public class RegistroAlunoIntegrationHandler(
    IServiceProvider serviceProvider,
    IMessageBus bus,
    ILogger<RegistroAlunoIntegrationHandler> logger) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    private readonly IMessageBus _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<RegistroAlunoIntegrationHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tentativas = 0;
        while (tentativas < 10 && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                DefinirResponder();
                _logger.LogInformation("Responder de RegistroUsuario configurado com sucesso");
                break;
            }
            catch (Exception ex)
            {
                tentativas++;
                _logger.LogWarning(ex, "Falha ao configurar responder (tentativa {Tentativa}). Aguardando RabbitMQ...", tentativas);
                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }
    }

    private void DefinirResponder()
    {
        _bus.RespondAsync<AlunoRegistradoIntegrationEvent, ResponseMessage>(async request => await ProcessarUsuarioRegistrado(request));
        _bus.AdvancedBus.Connected += OnConnect;
    }

    private void OnConnect(object? s, EventArgs e)
    {
        DefinirResponder();
    }

    private async Task<ResponseMessage> ProcessarUsuarioRegistrado(AlunoRegistradoIntegrationEvent message)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var integrationService = scope.ServiceProvider.GetRequiredService<IRegistroAlunoIntegrationService>();

            return await integrationService.ProcessarAlunoRegistradoAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar evento de usuário registrado. ID: {UserId}", message.Id);
            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Exception", ex.Message));
            return new ResponseMessage(validationResult);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Parando consumidor de eventos RegistroAlunoIntegrationHandler");
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
