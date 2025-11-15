using Conteudo.Application.Commands.AtualizarAula;

namespace Conteudo.UnitTests.Applications.Commands.AtualizarAula;
public class AtualizarAulaCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_campos_obrigatorios()
    {
        var cmd = new AtualizarAulaCommand(
            Guid.Empty, Guid.Empty, "", "", 0, 0, "", "", true, "");

        var result = new AtualizarAulaCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_campos_ok()
    {
        var cmd = new AtualizarAulaCommand(
            Guid.NewGuid(), Guid.NewGuid(), "Nome", "Desc", 1, 10, "url", "Vídeo", true, "");

        new AtualizarAulaCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
