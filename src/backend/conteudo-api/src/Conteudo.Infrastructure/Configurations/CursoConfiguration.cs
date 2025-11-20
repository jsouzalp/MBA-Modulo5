using Conteudo.Domain.Entities;
using Core.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Conteudo.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class CursoConfiguration : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> entity)
    {
        entity.ToTable("Cursos");

        entity.HasKey(a => a.Id)
            .HasName("CursosPK");

        entity.Property(x => x.Id)
            .HasColumnName("CursoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        entity.Property(c => c.Nome)
            .HasColumnName("Nome")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(c => c.Valor)
            .HasColumnName("Valor")
            .HasColumnType(DatabaseTypeConstant.Money)
            .IsRequired()
            .HasPrecision(10, 2);

        entity.Property(c => c.Nivel)
            .HasColumnName("Nivel")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(c => c.Instrutor)
            .HasColumnName("Instrutor")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(c => c.ImagemUrl)
            .HasColumnName("ImagemUrl")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(500);

        entity.Property(a => a.CreatedAt)
            .HasColumnName("DataCriacao")
            .HasColumnType(DatabaseTypeConstant.DateTime)
            .IsRequired();

        entity.Property(a => a.UpdatedAt)
            .HasColumnName("DataAlteracao")
            .HasColumnType(DatabaseTypeConstant.DateTime);

        // Configurar Value Object ConteudoProgramatico
        entity.OwnsOne(c => c.ConteudoProgramatico, cp =>
        {
            cp.Property(p => p.Resumo)
                .IsRequired()
                .HasColumnType(DatabaseTypeConstant.Varchar)
                .HasColumnName("ConteudoProgramatico_Resumo")
                .HasMaxLength(250);

            cp.Property(p => p.Descricao)
                .IsRequired()
                .HasColumnType(DatabaseTypeConstant.Varchar)
                .HasColumnName("ConteudoProgramatico_Descricao")
                .HasMaxLength(500);

            cp.Property(p => p.Objetivos)
                .IsRequired()
                .HasColumnType(DatabaseTypeConstant.Varchar)
                .HasColumnName("ConteudoProgramatico_Objetivos")
                .HasMaxLength(1024);

            cp.Property(p => p.PreRequisitos)
                .HasColumnType(DatabaseTypeConstant.Varchar)
                .HasColumnName("ConteudoProgramatico_PreRequisitos")
                .HasMaxLength(1024);

            cp.Property(p => p.PublicoAlvo)
                .HasColumnType(DatabaseTypeConstant.Varchar)
                .HasColumnName("ConteudoProgramatico_PublicoAlvo")
                .HasMaxLength(1024);

            cp.Property(p => p.Metodologia)
                .HasColumnType(DatabaseTypeConstant.Varchar)
                .HasColumnName("ConteudoProgramatico_Metodologia")
                .HasMaxLength(1024);

            cp.Property(p => p.Recursos)
                .HasColumnType(DatabaseTypeConstant.Varchar)
                .HasColumnName("ConteudoProgramatico_Recursos")
                .HasMaxLength(1024);

            cp.Property(p => p.Avaliacao)
                .HasColumnType(DatabaseTypeConstant.Varchar)
                .HasColumnName("ConteudoProgramatico_Avaliacao")
                .HasMaxLength(1024);

            cp.Property(p => p.Bibliografia)
                .HasColumnType(DatabaseTypeConstant.Varchar)
                .HasColumnName("ConteudoProgramatico_Bibliografia")
                .HasMaxLength(1024);
        });

        entity.HasIndex(c => c.Nome)
            .HasDatabaseName("CursoNomeIDX")
            .IsUnique();

        entity.HasIndex(c => c.ValidoAte)
            .HasDatabaseName("CursoValidoAteIDX");

        entity.HasIndex(c => c.CategoriaId)
            .HasDatabaseName("CursoCategoriaIdIDX");

        entity.HasOne(c => c.Categoria)
            .WithMany(cat => cat.Cursos)
            .HasForeignKey(c => c.CategoriaId)
            .OnDelete(DeleteBehavior.SetNull);

        entity.HasMany(c => c.Aulas)
            .WithOne(a => a.Curso)
            .HasForeignKey(a => a.CursoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
