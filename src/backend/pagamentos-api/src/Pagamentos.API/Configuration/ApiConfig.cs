using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Pagamentos.API.Configuration;

[ExcludeFromCodeCoverage]
public static class ApiConfig
{
    public static WebApplicationBuilder AddApiConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return builder;
    }
}
