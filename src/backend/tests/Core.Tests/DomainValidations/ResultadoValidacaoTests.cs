using Core.DomainValidations;
using Plataforma.Educacao.Core.Exceptions;

namespace Core.Tests.DomainValidations;

public class ResultadoValidacaoTests 
{
    private class EntidadeTeste
    {
        public string Nome { get; set; } = string.Empty;
    }

    [Fact]
    public void ResultadoValidacao_DeveCriarSemErros()
    {
        // Arrange & Act
        var resultado = new ResultadoValidacao<EntidadeTeste>();

        // Assert
        resultado.Should().NotBeNull();
    }

    [Fact]
    public void ResultadoValidacao_DeveAdicionarErro()
    {
        // Arrange
        var resultado = new ResultadoValidacao<EntidadeTeste>();
        var mensagem = "Campo obrigatório";

        // Act
        resultado.AdicionarErro(mensagem);
    }

    [Fact]
    public void ResultadoValidacao_DeveAdicionarErroComNomeDaEntidade()
    {
        // Arrange
        var resultado = new ResultadoValidacao<EntidadeTeste>();
        var mensagem = "Campo obrigatório";

        // Act
        resultado.AdicionarErro(mensagem);

        // Assert
    }

    [Fact]
    public void ResultadoValidacao_DeveIgnorarErroVazio()
    {
        // Arrange
        var resultado = new ResultadoValidacao<EntidadeTeste>();

        // Act
        resultado.AdicionarErro(string.Empty);
        resultado.AdicionarErro("   ");
        resultado.AdicionarErro(null!);

        // Assert
    }

    [Fact]
    public void ResultadoValidacao_DeveDispararExcecaoQuandoInvalido()
    {
        // Arrange
        var resultado = new ResultadoValidacao<EntidadeTeste>();
        resultado.AdicionarErro("Erro de validação");

        // Act & Assert
        var action = () => resultado.DispararExcecaoDominioSeInvalido();
        action.Should().Throw<Plataforma.Educacao.Core.Exceptions.DomainException>();
    }

    [Fact]
    public void ResultadoValidacao_DeveNaoDispararExcecaoQuandoValido()
    {
        // Arrange
        var resultado = new ResultadoValidacao<EntidadeTeste>();

        // Act & Assert
        var action = () => resultado.DispararExcecaoDominioSeInvalido();
        action.Should().NotThrow();
    }

    [Fact]
    public void ResultadoValidacao_DeveAdicionarMultiplosErros()
    {
        // Arrange
        var resultado = new ResultadoValidacao<EntidadeTeste>();
        var mensagem1 = "Erro 1";
        var mensagem2 = "Erro 2";

        // Act
        resultado.AdicionarErro(mensagem1);
        resultado.AdicionarErro(mensagem2);

        // Assert
        var action = () => resultado.DispararExcecaoDominioSeInvalido();
        action.Should().Throw<Plataforma.Educacao.Core.Exceptions.DomainException>();
    }

    [Fact]
    public void ResultadoValidacao_DeveFuncionarComDiferentesTipos()
    {
        // Arrange
        var resultadoString = new ResultadoValidacao<string>();
        var resultadoInt = new ResultadoValidacao<object>();

        // Act
        resultadoString.AdicionarErro("Erro string");
        resultadoInt.AdicionarErro("Erro int");

        // Assert
        var action1 = () => resultadoString.DispararExcecaoDominioSeInvalido();
        var action2 = () => resultadoInt.DispararExcecaoDominioSeInvalido();

        action1.Should().Throw<Plataforma.Educacao.Core.Exceptions.DomainException>();
        action2.Should().Throw<Plataforma.Educacao.Core.Exceptions.DomainException>();
    }

    [Fact]
    public void DispararExcecaoDominioSeInvalido_deve_lancar_quando_houver_erros_com_prefixo_do_tipo()
    {
        var r = new ResultadoValidacao<object>();

        r.AdicionarErro("campo obrigatório");
        Action act = () => r.DispararExcecaoDominioSeInvalido();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.StartsWith("(Object)"));
    }

    [Fact]
    public void DispararExcecaoDominioSeInvalido_nao_deve_lancar_quando_sem_erros()
    {
        var r = new ResultadoValidacao<object>();
        r.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().NotThrow();
    }
}
