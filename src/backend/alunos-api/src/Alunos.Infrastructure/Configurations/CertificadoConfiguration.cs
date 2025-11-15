using Alunos.Domain.Entities;
using Core.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Alunos.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class CertificadoConfiguration : IEntityTypeConfiguration<Certificado>
{
    public void Configure(EntityTypeBuilder<Certificado> builder)
    {
        builder.ToTable("Certificados");

        builder.HasKey(x => x.Id)
            .HasName("CertificadosPK");

        builder.Property(x => x.Id)
            .HasColumnName("CertificadoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.MatriculaCursoId)
            .HasColumnName("MatriculaCursoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.NomeCurso)
            .HasColumnName("NomeCurso")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.DataSolicitacao)
            .HasColumnName("DataSolicitacao")
            .HasColumnType(DatabaseTypeConstant.SmallDateTime)
            .IsRequired();

        builder.Property(x => x.DataEmissao)
            .HasColumnName("DataEmissao")
            .HasColumnType(DatabaseTypeConstant.SmallDateTime);

        builder.Property(x => x.CargaHoraria)
            .HasColumnName("CargaHoraria")
            .HasColumnType(DatabaseTypeConstant.Int16)
            .IsRequired();

        builder.Property(x => x.NotaFinal)
            .HasColumnName("NotaFinal")
            .HasColumnType(DatabaseTypeConstant.Byte)
            .IsRequired();

        builder.Property(x => x.PathCertificado)
            .HasColumnName("PathCertificado")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.NomeInstrutor)
            .HasColumnName("NomeInstrutor")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("DataCriacao")
            .HasColumnType(DatabaseTypeConstant.DateTime)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("DataAlteracao")
            .HasColumnType(DatabaseTypeConstant.DateTime);

        builder.HasIndex(x => x.MatriculaCursoId).HasDatabaseName("CertificadosMatriculaCursoIdIDX");

        builder.HasOne(x => x.MatriculaCurso)
           .WithOne(x => x.Certificado)
           .HasForeignKey<Certificado>(x => x.MatriculaCursoId)
           .HasConstraintName("CertificadoMatriculaCursoFK")
           .OnDelete(DeleteBehavior.Cascade);
    }
}
