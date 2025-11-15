using Conteudo.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Conteudo.UnitTests.Repositories.Infra;
public static class EfSqliteInMemoryFactory
{
    public static (DbContextOptions<ConteudoDbContext> options, SqliteConnection conn) Create()
    {
        var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();

        var options = new DbContextOptionsBuilder<ConteudoDbContext>()
            .UseSqlite(conn)
            .EnableSensitiveDataLogging()
            .Options;

        using (var ctx = new ConteudoDbContext(options))
        {
            ctx.Database.EnsureCreated();
        }

        return (options, conn);
    }
}
