using Alunos.Application.Commands.MatricularAluno;
using FluentAssertions;

namespace Alunos.Tests.Applications.MatricularAluno;
public class MatricularAlunoCommandTests
{
    [Fact]
    public void Ctor_deve_definir_RaizAgregacao_com_AlunoId()
    {
        var alunoId = Guid.NewGuid();
        var cmd = new MatricularAlunoCommand(alunoId, Guid.NewGuid(), true, "Curso", 10, "");

        cmd.RaizAgregacao.Should().Be(alunoId);
    }
}
