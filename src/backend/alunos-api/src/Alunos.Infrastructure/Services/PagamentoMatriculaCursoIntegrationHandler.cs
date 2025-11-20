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
public class PagamentoMatriculaCursoIntegrationHandler(IServiceProvider serviceProvider,
    IMessageBus bus,
    ILogger<PagamentoMatriculaCursoIntegrationHandler> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();
        return Task.CompletedTask;
    }

    private void SetResponder()
    {
        bus.RespondAsync<PagamentoMatriculaCursoIntegrationEvent, ResponseMessage>(async request => await ProcessarPagamentoMatriculaCurso(request));
        bus.AdvancedBus.Connected += OnConnect;
    }

    private void OnConnect(object? s, EventArgs e)
    {
        SetResponder();
    }

    private async Task<ResponseMessage> ProcessarPagamentoMatriculaCurso(PagamentoMatriculaCursoIntegrationEvent message)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var integrationService = scope.ServiceProvider.GetRequiredService<IRegistroPagamentoIntegrationService>();

            return await integrationService.ProcessarPagamentoMatriculaCursoAsync(message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar evento de pagamento de matrícula-curso. Id Aluno: {AlunoId} Id Curso: {CursoId}", message.AlunoId, message.CursoId);
            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Exception", ex.Message));
            return new ResponseMessage(validationResult);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Parando consumidor de eventos PagamentoMatriculaCursoIntegrationHandler");
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
