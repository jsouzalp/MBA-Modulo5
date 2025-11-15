using Conteudo.Domain.Entities;
using Conteudo.Infrastructure.Configurations;
using Core.Data;
using Core.DomainObjects;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Conteudo.Infrastructure.Data;

[ExcludeFromCodeCoverage]
public class ConteudoDbContext(DbContextOptions<ConteudoDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Aula> Aulas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Material> Materiais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AulaConfiguration());
        modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
        modelBuilder.ApplyConfiguration(new CursoConfiguration());
        modelBuilder.ApplyConfiguration(new MaterialConfiguration());

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

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Entidade &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (Entidade)entry.Entity;

            if (entry.State == EntityState.Modified)
            {
                entity.AtualizarDataModificacao();
            }
        }
    }

    public async Task<bool> Commit()
    {
        return await SaveChangesAsync() > 0;
    }
}
