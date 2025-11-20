using Conteudo.Application.Commands.ExcluirMaterial;

namespace Conteudo.UnitTests.Applications.Commands.ExcluirMaterial;
public class ExcluirMaterialCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_quando_Id_empty()
    {
        var result = new ExcluirMaterialCommandValidator().Validate(new ExcluirMaterialCommand(Guid.Empty));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_Id_ok()
    {
        new ExcluirMaterialCommandValidator().Validate(new ExcluirMaterialCommand(Guid.NewGuid()))
            .IsValid.Should().BeTrue();
    }
}
