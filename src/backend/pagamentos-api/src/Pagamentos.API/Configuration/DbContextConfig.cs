using Microsoft.EntityFrameworkCore;
using Pagamentos.Infrastructure.Context;
using System.Diagnostics.CodeAnalysis;

namespace Pagamentos.API.Configuration;

[ExcludeFromCodeCoverage]
public static class DbContextConfig
{
    public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Test") || builder.Environment.IsEnvironment("Docker"))
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<PagamentoContext>(options =>
                options.UseSqlite(connectionString));

            return builder;
        }
        else
        {
            builder.Services.AddDbContext<PagamentoContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            return builder;
        }
    }
}
