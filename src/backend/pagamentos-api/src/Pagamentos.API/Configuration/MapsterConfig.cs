using Mapster;
using System.Diagnostics.CodeAnalysis;

namespace Pagamentos.API.Configuration;

[ExcludeFromCodeCoverage]
public static class MapsterConfig
{
    public static WebApplicationBuilder AddMapsterConfiguration(this WebApplicationBuilder builder)
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);
        return builder;
    }
}
