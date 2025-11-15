using Conteudo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Conteudo.API.Configuration;

[ExcludeFromCodeCoverage]
public static class DbContextConfiguration
{
    public static WebApplicationBuilder AddDbContextConfiguration(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Docker"))
        {
            builder.Services.AddDbContext<ConteudoDbContext>(opt =>
            {
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }
        else
        {
            builder.Services.AddDbContext<ConteudoDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }
        return builder;
    }
}
