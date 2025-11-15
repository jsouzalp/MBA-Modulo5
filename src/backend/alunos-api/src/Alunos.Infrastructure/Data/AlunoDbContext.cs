using Alunos.Domain.Entities;
using Alunos.Infrastructure.Configurations;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Alunos.Infrastructure.Data;

[ExcludeFromCodeCoverage]
public class AlunoDbContext(DbContextOptions<AlunoDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<MatriculaCurso> MatriculasCursos { get; set; }
    public DbSet<Certificado> Certificados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AlunoConfiguration());
        modelBuilder.ApplyConfiguration(new CertificadoConfiguration());
        modelBuilder.ApplyConfiguration(new MatriculaCursoConfiguration());

        foreach (var property in modelBuilder.Model.GetEntityTypes()
                                                   .SelectMany(e => e.GetProperties()
                                                   .Where(p => p.ClrType == typeof(string))))
        {
            if (property.GetMaxLength() == null)
            {
                property.SetMaxLength(0);
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
        return await base.SaveChangesAsync() > 0;
    }
}
