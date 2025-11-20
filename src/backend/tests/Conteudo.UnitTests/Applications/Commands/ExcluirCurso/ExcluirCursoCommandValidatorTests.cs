using Conteudo.Application.Commands.ExcluirCurso;

namespace Conteudo.UnitTests.Applications.Commands.ExcluirCurso;
public class ExcluirCursoCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_quando_Id_empty()
    {
        var result = new ExcluirCursoCommandValidator().Validate(new ExcluirCursoCommand(Guid.Empty));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_Id_ok()
    {
        new ExcluirCursoCommandValidator().Validate(new ExcluirCursoCommand(Guid.NewGuid()))
            .IsValid.Should().BeTrue();
    }
}
