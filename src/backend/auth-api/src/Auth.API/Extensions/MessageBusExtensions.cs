using Core.Utils;
using MessageBus;
using System.Diagnostics.CodeAnalysis;

namespace Auth.API.Extensions;

[ExcludeFromCodeCoverage]
public static class MessageBusExtensions
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBus(configuration?.GetMessageQueueConnection("MessageBus")!);
    }
}
