using Microsoft.EntityFrameworkCore;
using Pagamentos.Infrastructure.Context;
using System.Diagnostics.CodeAnalysis;

namespace Pagamentos.API.Configuration;

[ExcludeFromCodeCoverage]
public static class DbMigrationHelperExtension
{
    public static void UseDbMigrationHelper(this WebApplication app)
    {
        DbMigrationHelpers.EnsureSeedData(app).Wait();
    }
}

public static class DbMigrationHelpers
{
    public static async Task EnsureSeedData(WebApplication serviceScope)
    {
        var services = serviceScope.Services.CreateScope().ServiceProvider;
        await EnsureSeedData(services);
    }

    public static async Task EnsureSeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        var contextPagamento = scope.ServiceProvider.GetRequiredService<PagamentoContext>();

        if (env.IsDevelopment() || env.IsEnvironment("Test") || env.IsEnvironment("Docker"))
        {
            await MigrarBancosAsync(contextPagamento);
            await EnsureSeedProducts(serviceProvider);
        }
    }

    private static async Task MigrarBancosAsync(DbContext contextPagamento)
    {
        await contextPagamento.Database.MigrateAsync();
    }

    private static Task EnsureSeedProducts(IServiceProvider serviceProvider)
    {
        return Task.CompletedTask;
    }
}
