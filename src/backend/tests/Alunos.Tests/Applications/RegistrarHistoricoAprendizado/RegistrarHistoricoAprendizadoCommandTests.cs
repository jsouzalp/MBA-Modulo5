using Alunos.Application.Commands.RegistrarHistoricoAprendizado;
using FluentAssertions;

namespace Alunos.Tests.Applications.RegistrarHistoricoAprendizado;
public class RegistrarHistoricoAprendizadoCommandTests
{
    [Fact]
    public void Ctor_deve_definir_RaizAgregacao_com_AlunoId()
    {
        var aluno = Guid.NewGuid();
        var cmd = new RegistrarHistoricoAprendizadoCommand(aluno, Guid.NewGuid(), Guid.NewGuid(), "A1", 10);

        cmd.RaizAgregacao.Should().Be(aluno);
    }
}
