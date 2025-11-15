using Alunos.Application.Commands.CadastrarAluno;
using FluentAssertions;

namespace Alunos.Tests.Applications.CadastrarAluno;
public class CadastrarAlunoCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_campos_e_limites()
    {
        var cmd = new CadastrarAlunoCommand(Guid.Empty, "", "x", "", DateTime.Today.AddDays(1),
                                            new string('x', 26), new string('x', 21), new string('x', 51),
                                            "SPX", new string('x', 9), new string('x', 1025));

        var result = new CadastrarAlunoCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_campos_ok()
    {
        var cmd = new CadastrarAlunoCommand(Guid.NewGuid(), "Fulano", "f@e.com", "12345678909",
                                            DateTime.Today.AddYears(-18), "11", "M", "SP", "SP", "01001000", "https://cdn.exemplo.com/foto.png");

        new CadastrarAlunoCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
