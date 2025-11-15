using Alunos.Domain.Entities;
using Core.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Alunos.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class MatriculaCursoConfiguration : IEntityTypeConfiguration<MatriculaCurso>
{
    public void Configure(EntityTypeBuilder<MatriculaCurso> builder)
    {
        builder.ToTable("MatriculasCursos");

        builder.HasKey(x => x.Id)
            .HasName("MatriculasCursosPK");

        builder.Property(x => x.Id)
            .HasColumnName("MatriculaCursoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.AlunoId)
            .HasColumnName("AlunoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.CursoId)
            .HasColumnName("CursoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.NomeCurso)
            .HasColumnName("NomeCurso")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Valor)
            .HasColumnName("Valor")
            .HasColumnType(DatabaseTypeConstant.Money)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.DataMatricula)
            .HasColumnName("DataMatricula")
            .HasColumnType(DatabaseTypeConstant.SmallDateTime)
            .IsRequired();

        builder.Property(x => x.DataConclusao)
            .HasColumnName("DataConclusao")
            .HasColumnType(DatabaseTypeConstant.SmallDateTime);

        builder.Property(x => x.EstadoMatricula)
            .HasColumnName("EstadoMatricula")
            .HasColumnType(DatabaseTypeConstant.Byte)
            .IsRequired();

        builder.Property(x => x.Observacao)
            .HasColumnName("Observacao")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(2000);

        builder.OwnsMany(x => x.HistoricoAprendizado, ha =>
        {
            ha.ToTable("HistoricosAprendizado");
            ha.WithOwner().HasForeignKey(h => h.MatriculaCursoId);
            ha.HasKey(x => x.Id).HasName("HistoricoAprendizadoPK");

            ha.Property(x => x.Id).HasColumnName("HistoricoAprendizadoId").HasColumnType(DatabaseTypeConstant.UniqueIdentifier).IsRequired();
            ha.Property(x => x.MatriculaCursoId).HasColumnName("MatriculaCursoId").HasColumnType(DatabaseTypeConstant.UniqueIdentifier).IsRequired();
            ha.Property(x => x.CursoId).HasColumnType(DatabaseTypeConstant.UniqueIdentifier).IsRequired();
            ha.Property(x => x.AulaId).HasColumnType(DatabaseTypeConstant.UniqueIdentifier).IsRequired();
            ha.Property(x => x.NomeAula).HasColumnType(DatabaseTypeConstant.Varchar).HasMaxLength(100).IsRequired();
            ha.Property(x => x.CargaHoraria).HasColumnType(DatabaseTypeConstant.Int32).IsRequired();
            ha.Property(x => x.DataInicio).HasColumnType(DatabaseTypeConstant.SmallDateTime).IsRequired();
            ha.Property(x => x.DataTermino).HasColumnType(DatabaseTypeConstant.SmallDateTime);
            ha.HasIndex(x => x.CursoId).HasDatabaseName("HistoricosAprendizadoCursoIdIDX");
            ha.HasIndex(x => x.AulaId).HasDatabaseName("HistoricosAprendizadoAulaIdIDX");
        });

        builder.Property(x => x.CreatedAt)
            .HasColumnName("DataCriacao")
            .HasColumnType(DatabaseTypeConstant.DateTime)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("DataAlteracao")
            .HasColumnType(DatabaseTypeConstant.DateTime);

        builder.HasIndex(x => x.AlunoId).HasDatabaseName("MatriculasCursosAlunoIdIDX");
        builder.HasIndex(x => x.CursoId).HasDatabaseName("MatriculasCursosCursoIdIDX");

        builder.HasOne(x => x.Certificado)
           .WithOne(x => x.MatriculaCurso)
           .HasForeignKey<Certificado>(x => x.MatriculaCursoId)
           .HasConstraintName("MatriculasCursosCertificadosFK")
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Aluno)
           .WithMany(x => x.MatriculasCursos)
           .HasForeignKey(x => x.AlunoId)
           .HasConstraintName("MatriculasCursosAlunosFK")
           .OnDelete(DeleteBehavior.Cascade);
    }
}
