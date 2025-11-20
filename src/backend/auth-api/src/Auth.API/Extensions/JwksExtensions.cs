using Auth.Infrastructure.Data;
using System.Diagnostics.CodeAnalysis;

namespace Auth.API.Extensions;

[ExcludeFromCodeCoverage]
public static class JwksExtensions
{
    public static IServiceCollection AddJwksConfiguration(this IServiceCollection services)
    {
        services.AddJwksManager()
                .UseJwtValidation()
                .PersistKeysToDatabaseStore<AuthDbContext>();

        return services;
    }
}
