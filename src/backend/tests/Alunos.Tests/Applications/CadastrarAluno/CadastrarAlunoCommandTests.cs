using Alunos.Application.Commands.CadastrarAluno;
using FluentAssertions;

namespace Alunos.Tests.Applications.CadastrarAluno;
public class CadastrarAlunoCommandTests
{
    [Fact]
    public void Ctor_deveria_definir_RaizAgregacao_com_Id()
    {
        var id = Guid.NewGuid();
        var cmd = new CadastrarAlunoCommand(id, "n", "e@e.com", "12345678909", DateTime.Today.AddYears(-18), "", "M", "SP", "SP", "01001000", "");

        // Esperado: RaizAgregacao == id
        cmd.RaizAgregacao.Should().Be(id);
    }
}
