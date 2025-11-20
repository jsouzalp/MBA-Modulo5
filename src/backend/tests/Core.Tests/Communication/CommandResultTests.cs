using Core.Communication;
using FluentValidation.Results;

namespace Core.Tests.Communication;

public class CommandResultTests
{
    [Fact]
    public void CommandResult_DeveCriarComValidationResult()
    {
        // Arrange
        var validationResult = new ValidationResult();

        // Act
        var commandResult = new CommandResult(validationResult);

        // Assert
        commandResult.Should().NotBeNull();
        commandResult.IsValid.Should().BeTrue();
        commandResult.Data.Should().BeNull();
    }

    [Fact]
    public void CommandResult_DeveCriarComValidationResultEData()
    {
        // Arrange
        var validationResult = new ValidationResult();
        var data = new { Id = 1, Nome = "Teste" };

        // Act
        var commandResult = new CommandResult(validationResult, data);

        // Assert
        commandResult.Should().NotBeNull();
        commandResult.IsValid.Should().BeTrue();
        commandResult.Data.Should().Be(data);
    }

    [Fact]
    public void CommandResult_DeveSerInvalidoComErros()
    {
        // Arrange
        var validationResult = new ValidationResult();
        validationResult.Errors.Add(new ValidationFailure("", "Erro de teste"));

        // Act
        var commandResult = new CommandResult(validationResult);

        // Assert
        commandResult.Should().NotBeNull();
        commandResult.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CommandResult_DeveAtualizarValidationResult()
    {
        // Arrange
        var validationResult1 = new ValidationResult();
        var validationResult2 = new ValidationResult();
        validationResult2.Errors.Add(new ValidationFailure("", "Novo erro"));

        var commandResult = new CommandResult(validationResult1);

        // Act
        commandResult.AtualizarValidationResult(validationResult2);

        // Assert
        commandResult.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CommandResult_DeveAdicionarErro()
    {
        // Arrange
        var validationResult = new ValidationResult();
        var commandResult = new CommandResult(validationResult);
        var mensagem = "Erro adicionado";

        // Act
        commandResult.AdicionarErro(mensagem);

        // Assert
        commandResult.IsValid.Should().BeFalse();
        commandResult.ObterErros().Should().Contain(mensagem);
    }

    [Fact]
    public void CommandResult_DeveObterErros()
    {
        // Arrange
        var validationResult = new ValidationResult();
        var commandResult = new CommandResult(validationResult);
        var mensagem1 = "Erro 1";
        var mensagem2 = "Erro 2";

        commandResult.AdicionarErro(mensagem1);
        commandResult.AdicionarErro(mensagem2);

        // Act
        var erros = commandResult.ObterErros();

        // Assert
        erros.Should().HaveCount(2);
        erros.Should().Contain(mensagem1);
        erros.Should().Contain(mensagem2);
    }

    [Fact]
    public void CommandResult_DeveObterValidationResult()
    {
        // Arrange
        var validationResult = new ValidationResult();
        var commandResult = new CommandResult(validationResult);

        // Act
        var result = commandResult.ObterValidationResult();

        // Assert
        result.Should().Be(validationResult);
    }

    [Fact]
    public void CommandResult_DeveSerValidoSemErros()
    {
        // Arrange
        var validationResult = new ValidationResult();
        var commandResult = new CommandResult(validationResult);

        // Act & Assert
        commandResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Ctor_deve_refletir_ValidationResult_e_Data()
    {
        var vr = new ValidationResult(); // IsValid = true
        var cr = new CommandResult(vr, data: 123);

        cr.IsValid.Should().BeTrue();
        cr.Data.Should().Be(123);
        cr.ObterValidationResult().Should().BeSameAs(vr);
    }

    [Fact]
    public void AdicionarErro_deve_incluir_mensagem_e_IsValid_ficar_false()
    {
        var cr = new CommandResult(new ValidationResult());
        cr.AdicionarErro("boom");

        cr.IsValid.Should().BeFalse();
        cr.ObterErros().Should().ContainSingle().Which.Should().Be("boom");
    }

    [Fact]
    public void AtualizarValidationResult_deve_trocar_instancia_e_refletir_em_IsValid()
    {
        var vrTrue = new ValidationResult();               // válido
        var vrFalse = new ValidationResult(new[] { new ValidationFailure("x", "y") }); // inválido

        var cr = new CommandResult(vrTrue);
        cr.IsValid.Should().BeTrue();

        cr.AtualizarValidationResult(vrFalse);
        cr.ObterValidationResult().Should().BeSameAs(vrFalse);
        cr.IsValid.Should().BeFalse();
    }
}
