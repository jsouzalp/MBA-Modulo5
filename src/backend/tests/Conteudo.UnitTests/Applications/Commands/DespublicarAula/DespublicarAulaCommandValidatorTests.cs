using Conteudo.Application.Commands.DespublicarAula;

namespace Conteudo.UnitTests.Applications.Commands.DespublicarAula;
public class DespublicarAulaCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_quando_Id_for_empty()
    {
        var cmd = new DespublicarAulaCommand(Guid.NewGuid(), Guid.Empty);

        var result = new DespublicarAulaCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_Id_ok()
    {
        var cmd = new DespublicarAulaCommand(Guid.NewGuid(), Guid.NewGuid());
        new DespublicarAulaCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
