using Alunos.Application.Commands.AtualizarPagamento;
using FluentAssertions;

namespace Alunos.Tests.Applications.AtualizarPagamento;
public class AtualizarPagamentoMatriculaCommandTests
{
    [Fact]
    public void Ctor_deve_definir_RaizAgregacao_com_AlunoId()
    {
        var alunoId = Guid.NewGuid();
        var cmd = new AtualizarPagamentoMatriculaCommand(alunoId, Guid.NewGuid());

        cmd.RaizAgregacao.Should().Be(alunoId);
    }
}
