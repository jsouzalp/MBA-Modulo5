using Alunos.Application.Commands.MatricularAluno;
using FluentAssertions;

namespace Alunos.Tests.Applications.MatricularAluno;
public class MatricularAlunoCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_campos_quebrados()
    {
        var cmd = new MatricularAlunoCommand(Guid.Empty, Guid.Empty, false, "", 0m, "");
        var result = new MatricularAlunoCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_campos_ok()
    {
        var cmd = new MatricularAlunoCommand(Guid.NewGuid(), Guid.NewGuid(), true, "DDD", 100m, "obs");
        new MatricularAlunoCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
