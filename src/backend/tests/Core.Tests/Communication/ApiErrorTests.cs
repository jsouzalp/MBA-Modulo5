using Core.Communication;

namespace Core.Tests.Communication;

public class ApiErrorTests
{
    [Fact]
    public void ApiError_DeveCriarComPropriedadesPadrao()
    {
        // Arrange & Act
        var apiError = new ApiError();

        // Assert
        apiError.Should().NotBeNull();
        apiError.Message.Should().Be(string.Empty);
        apiError.Details.Should().BeNull();
    }

    [Fact]
    public void ApiError_DevePermitirDefinirMensagem()
    {
        // Arrange
        var mensagem = "Erro de validação";
        var apiError = new ApiError();

        // Act
        apiError.Message = mensagem;

        // Assert
        apiError.Message.Should().Be(mensagem);
    }

    [Fact]
    public void ApiError_DevePermitirDefinirDetalhes()
    {
        // Arrange
        var detalhes = new[] { "Campo obrigatório", "Formato inválido" };
        var apiError = new ApiError();

        // Act
        apiError.Details = detalhes;

        // Assert
        apiError.Details.Should().BeEquivalentTo(detalhes);
    }

    [Fact]
    public void ApiError_DevePermitirDetalhesVazio()
    {
        // Arrange
        var apiError = new ApiError();

        // Act
        apiError.Details = new string[0];

        // Assert
        apiError.Details.Should().NotBeNull();
        apiError.Details.Should().BeEmpty();
    }

    [Fact]
    public void ApiError_DevePermitirDetalhesNull()
    {
        // Arrange
        var apiError = new ApiError();

        // Act
        apiError.Details = null;

        // Assert
        apiError.Details.Should().BeNull();
    }
}
