using Core.Utils;
using MessageBus;
using System.Diagnostics.CodeAnalysis;

namespace Pagamentos.API.Configuration;

[ExcludeFromCodeCoverage]
public static class MessageBusConfig
{
    public static WebApplicationBuilder AddMessageBusConfiguration(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddMessageBus(configuration?.GetMessageQueueConnection("MessageBus")!);

        return builder;
    }
}
