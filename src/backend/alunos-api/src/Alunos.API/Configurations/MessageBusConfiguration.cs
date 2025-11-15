using Alunos.Infrastructure.Services;
using Core.Utils;
using MessageBus;
using System.Diagnostics.CodeAnalysis;

namespace Alunos.API.Configurations;

[ExcludeFromCodeCoverage]
public static class MessageBusConfiguration
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBus(configuration?.GetMessageQueueConnection("MessageBus")!)
            .AddHostedService<RegistroAlunoIntegrationHandler>();

        services.AddMessageBus(configuration?.GetMessageQueueConnection("MessageBus")!)
            .AddHostedService<PagamentoMatriculaCursoIntegrationHandler>();
    }
}
