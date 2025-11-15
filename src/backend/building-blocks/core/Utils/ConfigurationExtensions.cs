using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Core.Utils;

[ExcludeFromCodeCoverage]
public static class ConfigurationExtensions
{
    public static string GetMessageQueueConnection(this IConfiguration configuration, string name)
    {
        return configuration?.GetSection("MessageQueueConnection")?[name]!;
    }
}
