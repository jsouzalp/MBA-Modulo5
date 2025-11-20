using Conteudo.Application.Commands.PublicarAula;

namespace Conteudo.UnitTests.Applications.Commands.PublicarAula;
public class PublicarAulaCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_quando_Id_da_aula_for_empty()
    {
        var cmd = new PublicarAulaCommand(Guid.NewGuid(), Guid.Empty);
        var validator = new PublicarAulaCommandValidator();

        var result = validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_campos_ok()
    {
        var cmd = new PublicarAulaCommand(Guid.NewGuid(), Guid.NewGuid());
        var validator = new PublicarAulaCommandValidator();

        validator.Validate(cmd).IsValid.Should().BeTrue();
    }
}
