using Conteudo.Application.Commands.AtualizarMaterial;

namespace Conteudo.UnitTests.Applications.Commands.AtualizarMaterial;
public class AtualizarMaterialCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_campos_quebrados()
    {
        var cmd = new AtualizarMaterialCommand(
            Guid.NewGuid(), Guid.Empty, "", "", "", "", true, -1, ".pdf", -1);

        var result = new AtualizarMaterialCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_campos_ok()
    {
        var cmd = new AtualizarMaterialCommand(
            Guid.NewGuid(), Guid.NewGuid(), "Nome", "Desc", "PDF", "u", false, 0, ".pdf", 0);

        new AtualizarMaterialCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
