using Conteudo.Application.Commands.CadastrarAula;

namespace Conteudo.UnitTests.Applications.Commands.CadastrarAula;
public class CadastrarAulaCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_quando_campos_quebrados()
    {
        var cmd = new CadastrarAulaCommand(Guid.Empty, "", "", 0, 0, "", "", true, "");
        var result = new CadastrarAulaCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_campos_ok()
    {
        var cmd = new CadastrarAulaCommand(Guid.NewGuid(), "Nome", "Desc", 1, 15, "url", "Vídeo", true, "");
        new CadastrarAulaCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
