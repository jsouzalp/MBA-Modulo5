using Alunos.Domain.Entities;
using Core.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Alunos.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> builder)
    {
        builder.ToTable("Alunos");

        builder.HasKey(x => x.Id)
            .HasName("AlunosPK");

        builder.Property(x => x.Id)
            .HasColumnName("AlunoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.CodigoUsuarioAutenticacao)
            .HasColumnName("CodigoUsuarioAutenticacao")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.Nome)
            .HasColumnName("Nome")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Cpf)
            .HasColumnName("Cpf")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(11);

        builder.Property(x => x.DataNascimento)
            .HasColumnName("DataNascimento")
            .HasColumnType(DatabaseTypeConstant.SmallDateTime)
            .IsRequired();

        builder.Property(x => x.Telefone)
            .HasColumnName("Telefone")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(25);

        builder.Property(x => x.Ativo)
            .HasColumnName("Ativo")
            .HasColumnType(DatabaseTypeConstant.Boolean)
            .IsRequired();

        builder.Property(x => x.Genero)
            .HasColumnName("Genero")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Cidade)
            .HasColumnName("Cidade")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasColumnName("Estado")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(2);

        builder.Property(x => x.Cep)
            .HasColumnName("Cep")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(8)
            .IsRequired();

        builder.Property(x => x.Foto)
            .HasColumnName("Foto")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(1024);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("DataCriacao")
            .HasColumnType(DatabaseTypeConstant.DateTime)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("DataAlteracao")
            .HasColumnType(DatabaseTypeConstant.DateTime);

        builder.HasIndex(x => x.Nome).HasDatabaseName("AlunosNomeIDX");

        builder.HasIndex(x => x.Email)
               .IsUnique()
               .HasDatabaseName("AlunosEmailUK");

        builder.HasMany(x => x.MatriculasCursos)
           .WithOne(x => x.Aluno)
           .HasForeignKey(x => x.AlunoId)
           .HasConstraintName("AlunosMatriculaCursoFK")
           .OnDelete(DeleteBehavior.Cascade);
    }
}
