using Conteudo.Domain.Entities;
using Core.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Conteudo.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class AulaConfiguration : IEntityTypeConfiguration<Aula>
{
    public void Configure(EntityTypeBuilder<Aula> entity)
    {
        entity.ToTable("Aulas");

        entity.HasKey(a => a.Id)
            .HasName("AulasPK");

        entity.Property(x => x.Id)
            .HasColumnName("AulaId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        entity.Property(a => a.Nome)
            .HasColumnName("Nome")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(a => a.Descricao)
            .HasColumnName("Descricao")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(1024)
            .IsRequired();

        entity.Property(a => a.VideoUrl)
            .HasColumnName("VideoUrl")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .IsRequired()
            .HasMaxLength(500);

        entity.Property(a => a.TipoAula)
            .HasColumnName("TipoAula")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(50)
            .IsRequired();

        entity.Property(a => a.Observacoes)
            .HasColumnName("Observacoes")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(1000);

        entity.Property(a => a.CreatedAt)
            .HasColumnName("DataCriacao")
            .HasColumnType(DatabaseTypeConstant.DateTime)
            .IsRequired();

        entity.Property(a => a.UpdatedAt)
            .HasColumnName("DataAlteracao")
            .HasColumnType(DatabaseTypeConstant.DateTime);

        entity.HasIndex(x => x.Nome)
            .HasDatabaseName("AlunosNomeIDX");

        entity.HasIndex(a => new { a.CursoId, a.Numero })
            .HasDatabaseName("AlunosCursoIdNumeroIDX")
            .IsUnique();

        entity.HasIndex(a => a.IsPublicada)
            .HasDatabaseName("AlunosIsPublicadaIDX");

        entity.HasIndex(a => a.TipoAula)
            .HasDatabaseName("AlunosTipoAulaIDX");

        entity.HasOne(a => a.Curso)
            .WithMany(c => c.Aulas)
            .HasForeignKey(a => a.CursoId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(a => a.Materiais)
            .WithOne(m => m.Aula)
            .HasForeignKey(m => m.AulaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
