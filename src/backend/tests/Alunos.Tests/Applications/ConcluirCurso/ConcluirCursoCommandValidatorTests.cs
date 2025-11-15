using Alunos.Application.Commands.ConcluirCurso;
using Core.SharedDtos.Conteudo;
using FluentAssertions;

namespace Alunos.Tests.Applications.ConcluirCurso;
public class ConcluirCursoCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_campos_obrigatorios()
    {
        var cmd = new ConcluirCursoCommand(Guid.Empty, Guid.Empty, null!);
        var result = new ConcluirCursoCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_campos_ok()
    {
        var cmd = new ConcluirCursoCommand(Guid.NewGuid(), Guid.NewGuid(), new CursoDto { Id = Guid.NewGuid(), Aulas = [] });
        new ConcluirCursoCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
