using Core.Messages;
using FluentValidation.Results;

namespace Core.Tests.Messages;

public class ResponseMessageTests
{
    [Fact]
    public void ResponseMessage_DeveCriarComValidationResult()
    {
        // Arrange
        var validationResult = new ValidationResult();

        // Act
        var responseMessage = new ResponseMessage(validationResult);

        // Assert
        responseMessage.Should().NotBeNull();
        responseMessage.ValidationResult.Should().Be(validationResult);
    }

    [Fact]
    public void ResponseMessage_DevePermitirValidationResultComErros()
    {
        // Arrange
        var validationResult = new ValidationResult();
        validationResult.Errors.Add(new ValidationFailure("", "Erro de teste"));

        // Act
        var responseMessage = new ResponseMessage(validationResult);

        // Assert
        responseMessage.Should().NotBeNull();
        responseMessage.ValidationResult.Should().Be(validationResult);
        responseMessage.ValidationResult.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void ResponseMessage_DevePermitirValidationResultVazio()
    {
        // Arrange
        var validationResult = new ValidationResult();

        // Act
        var responseMessage = new ResponseMessage(validationResult);

        // Assert
        responseMessage.Should().NotBeNull();
        responseMessage.ValidationResult.Should().Be(validationResult);
        responseMessage.ValidationResult.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ResponseMessage_DevePermitirAlteracaoValidationResult()
    {
        // Arrange
        var validationResult1 = new ValidationResult();
        var validationResult2 = new ValidationResult();
        validationResult2.Errors.Add(new ValidationFailure("", "Novo erro"));

        var responseMessage = new ResponseMessage(validationResult1);

        // Act
        responseMessage.ValidationResult = validationResult2;

        // Assert
        responseMessage.ValidationResult.Should().Be(validationResult2);
        responseMessage.ValidationResult.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void ResponseMessage_DevePermitirValidationResultNull()
    {
        // Arrange
        var validationResult = new ValidationResult();
        var responseMessage = new ResponseMessage(validationResult);

        // Act
        responseMessage.ValidationResult = null!;

        // Assert
        responseMessage.ValidationResult.Should().BeNull();
    }

    [Fact]
    public void Construtor_deve_guardar_ValidationResult()
    {
        var vr = new ValidationResult();
        var rm = new ResponseMessage(vr);

        rm.ValidationResult.Should().BeSameAs(vr);
    }

    [Fact]
    public void Property_set_deve_permitir_troca_de_ValidationResult()
    {
        var rm = new ResponseMessage(new ValidationResult());
        var novo = new ValidationResult();

        rm.ValidationResult = novo;

        rm.ValidationResult.Should().BeSameAs(novo);
    }
}
