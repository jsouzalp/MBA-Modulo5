using Alunos.Application.Commands.SolicitarCertificado;
using FluentAssertions;

namespace Alunos.Tests.Applications.SolicitarCertificado;
public class SolicitarCertificadoCommandTests
{
    [Fact]
    public void Ctor_deve_definir_RaizAgregacao_com_AlunoId()
    {
        var aluno = Guid.NewGuid();
        var cmd = new SolicitarCertificadoCommand(aluno, Guid.NewGuid());

        cmd.RaizAgregacao.Should().Be(aluno);
    }
}
