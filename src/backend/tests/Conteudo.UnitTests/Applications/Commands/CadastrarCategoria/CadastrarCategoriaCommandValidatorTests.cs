using Conteudo.Application.Commands.CadastrarCategoria;

namespace Conteudo.UnitTests.Applications.Commands.CadastrarCategoria;
public class CadastrarCategoriaCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_campos_obrigatorios_e_limites()
    {
        var cmd = new CadastrarCategoriaCommand { Nome = "", Descricao = "", Cor = "", IconeUrl = new string('x', 600) };
        var result = new CadastrarCategoriaCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_campos_ok()
    {
        var cmd = new CadastrarCategoriaCommand { Nome = "Ok", Descricao = "d", Cor = "#abcdef", IconeUrl = "" };
        new CadastrarCategoriaCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
