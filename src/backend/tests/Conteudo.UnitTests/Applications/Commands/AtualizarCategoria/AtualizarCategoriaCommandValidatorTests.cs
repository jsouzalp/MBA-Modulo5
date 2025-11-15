using Conteudo.Application.Commands.AtualizarCategoria;

namespace Conteudo.UnitTests.Applications.Commands.AtualizarCategoria;
public class AtualizarCategoriaCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_campos_obrigatorios_e_formatos()
    {
        var cmd = new AtualizarCategoriaCommand
        {
            Id = Guid.Empty,
            Nome = "",
            Descricao = "",
            Cor = "abc" // não-hex
        };

        var result = new AtualizarCategoriaCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_campos_ok()
    {
        var cmd = new AtualizarCategoriaCommand
        {
            Id = Guid.NewGuid(),
            Nome = "Nome",
            Descricao = "Desc",
            Cor = "#A1B2C3",
            IconeUrl = new string('x', 10),
            Ordem = 0
        };

        new AtualizarCategoriaCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
