using Core.Utils;
using System.ComponentModel;

namespace Core.Tests.Utils;

public class EnumeratorExtensionTests
{
    public enum StatusTeste
    {
        [Description("Ativo")]
        Ativo,

        [Description("Inativo")]
        Inativo,

        [Description("Pendente")]
        Pendente,

        SemDescricao
    }

    public enum OutroEnum
    {
        [Description("Valor A")]
        ValorA,

        [Description("Valor B")]
        ValorB
    }

    public enum EnumSemDescricao
    {
        Valor1,
        Valor2
    }

    [Fact]
    public void GetDescription_DeveRetornarDescricaoQuandoAtributoExiste()
    {
        // Arrange
        var status = StatusTeste.Ativo;

        // Act
        var result = status.ObterDescricao();

        // Assert
        result.Should().Be("Ativo");
    }

    [Fact]
    public void GetDescription_DeveRetornarDescricaoParaDiferentesValores()
    {
        // Arrange
        var status1 = StatusTeste.Inativo;
        var status2 = StatusTeste.Pendente;

        // Act
        var result1 = status1.ObterDescricao();
        var result2 = status2.ObterDescricao();

        // Assert
        result1.Should().Be("Inativo");
        result2.Should().Be("Pendente");
    }

    [Fact]
    public void GetDescription_DeveRetornarValorEnumQuandoSemDescricao()
    {
        // Arrange
        var status = StatusTeste.SemDescricao;

        // Act
        var result = status.ObterDescricao();

        // Assert
        result.Should().Be("SemDescricao");
    }

    [Fact]
    public void GetDescription_DeveRetornarNaoInformadoQuandoEnumNull()
    {
        // Arrange
        StatusTeste? status = null;

        // Act
        var result = status.ObterDescricao();

        // Assert
        result.Should().Be("Não informado");
    }

    [Fact]
    public void GetDescription_DeveRetornarStringVaziaQuandoAtributoExisteMasDescricaoVazia()
    {
        // Arrange
        var status = StatusTeste.SemDescricao;

        // Act
        var result = status.ObterDescricao();

        // Assert
        result.Should().Be("SemDescricao");
    }

    [Fact]
    public void GetDescription_DeveFuncionarComDiferentesEnums()
    {
        // Arrange
        var valor1 = OutroEnum.ValorA;
        var valor2 = OutroEnum.ValorB;

        // Act
        var result1 = valor1.ObterDescricao();
        var result2 = valor2.ObterDescricao();

        // Assert
        result1.Should().Be("Valor A");
        result2.Should().Be("Valor B");
    }

    [Fact]
    public void GetDescription_DeveRetornarValorEnumQuandoAtributoDescriptionNaoExiste()
    {
        // Arrange
        var valor = EnumSemDescricao.Valor1;

        // Act
        var result = valor.ObterDescricao();

        // Assert
        result.Should().Be("Valor1");
    }
}
