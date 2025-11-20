using Conteudo.Application.Commands.ExcluirAula;

namespace Conteudo.UnitTests.Applications.Commands.ExcluirAula;
public class ExcluirAulaCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_quando_Id_empty()
    {
        var cmd = new ExcluirAulaCommand(Guid.NewGuid(), Guid.Empty);
        var result = new ExcluirAulaCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_Id_ok()
    {
        var cmd = new ExcluirAulaCommand(Guid.NewGuid(), Guid.NewGuid());
        new ExcluirAulaCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
