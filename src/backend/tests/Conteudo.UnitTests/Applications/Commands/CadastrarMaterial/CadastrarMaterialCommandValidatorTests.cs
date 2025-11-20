using Conteudo.Application.Commands.CadastrarMaterial;

namespace Conteudo.UnitTests.Applications.Commands.CadastrarMaterial;
public class CadastrarMaterialCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_campos_quebrados()
    {
        var cmd = new CadastrarMaterialCommand(Guid.NewGuid(), Guid.Empty, "", "", "", "", true, -1, ".pdf", -1);

        var result = new CadastrarMaterialCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_campos_ok()
    {
        var cmd = new CadastrarMaterialCommand(Guid.NewGuid(), Guid.NewGuid(), "Nome", "Desc", "PDF", "u", false, 0, ".pdf", 0);

        new CadastrarMaterialCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
