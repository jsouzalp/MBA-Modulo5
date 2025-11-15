using Alunos.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Alunos.Tests.Repositories.Infra;
public static class EfSqliteInMemoryFactory
{
    public static (DbContextOptions<AlunoDbContext> options, SqliteConnection conn) Create()
    {
        var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();

        var options = new DbContextOptionsBuilder<AlunoDbContext>()
            .UseSqlite(conn)
            .EnableSensitiveDataLogging()
            .Options;

        using (var ctx = new AlunoDbContext(options))
        {
            ctx.Database.EnsureCreated();
        }

        return (options, conn);
    }
}
