using Alunos.Application.Commands.CadastrarAluno;
using Alunos.Application.Integration;
using Core.Mediator;
using Core.Messages.Integration;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace Alunos.Tests.Applications.Integrations;
public class RegistroAlunoIntegrationServiceTests
{
    private static void VerifyLog<T>(Mock<ILogger<T>> logger, LogLevel level, string contains, Times times)
    {
        logger.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v != null && v.ToString().Contains(contains) == true),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            times);
    }

    [Fact]
    public async Task Deve_enviar_CadastrarAlunoCommand_com_campos_mapeados_e_retornar_ok()
    {
        var mediator = new Mock<IMediatorHandler>();
        var logger = new Mock<ILogger<RegistroAlunoIntegrationService>>();
        var service = new RegistroAlunoIntegrationService(mediator.Object, logger.Object);

        var msg = new AlunoRegistradoIntegrationEvent(
            Guid.NewGuid(), "Nome", "e@e.com", "12345678909", new DateTime(1990, 1, 1),
            "11", "M", "SP", "SP", "01001000", "foto.png");

        var vr = new ValidationResult();

        mediator.Setup(m => m.EnviarComando(It.Is<CadastrarAlunoCommand>(c =>
            c.Id == msg.Id &&
            c.Nome == msg.Nome &&
            c.Email == msg.Email &&
            c.Cpf == msg.Cpf &&
            c.DataNascimento == msg.DataNascimento &&
            c.Telefone == msg.Telefone &&
            c.Genero == msg.Genero &&
            c.Cidade == msg.Cidade &&
            c.Estado == msg.Estado &&
            c.Cep == msg.Cep &&
            c.Foto == msg.Foto
        ))).ReturnsAsync(vr);

        var resp = await service.ProcessarAlunoRegistradoAsync(msg);

        resp.ValidationResult.Should().BeSameAs(vr);
        VerifyLog(logger, LogLevel.Warning, "Falha na validação do comando de registro", Times.Never());
    }

    [Fact]
    public async Task Deve_logar_warning_quando_validacao_invalida()
    {
        var mediator = new Mock<IMediatorHandler>();
        var logger = new Mock<ILogger<RegistroAlunoIntegrationService>>();
        var service = new RegistroAlunoIntegrationService(mediator.Object, logger.Object);

        var msg = new AlunoRegistradoIntegrationEvent(
            Guid.NewGuid(), "Nome", "e@e.com", "12345678909", new DateTime(1990, 1, 1),
            "11", "M", "SP", "SP", "01001000", "foto.png");

        var vr = new ValidationResult([new ValidationFailure("a", "b")]);

        mediator.Setup(m => m.EnviarComando(It.IsAny<CadastrarAlunoCommand>())).ReturnsAsync(vr);

        var resp = await service.ProcessarAlunoRegistradoAsync(msg);

        resp.ValidationResult.Should().BeSameAs(vr);
        VerifyLog(logger, LogLevel.Warning, "Falha na validação do comando de registro", Times.Once());
    }

    [Fact]
    public async Task Deve_logar_error_e_retornar_ResponseMessage_com_exception_em_caso_de_erro()
    {
        var mediator = new Mock<IMediatorHandler>();
        var logger = new Mock<ILogger<RegistroAlunoIntegrationService>>();
        var service = new RegistroAlunoIntegrationService(mediator.Object, logger.Object);

        var msg = new AlunoRegistradoIntegrationEvent(
            Guid.NewGuid(), "Nome", "e@e.com", "12345678909", new DateTime(1990, 1, 1),
            "11", "M", "SP", "SP", "01001000", "foto.png");

        mediator.Setup(m => m.EnviarComando(It.IsAny<CadastrarAlunoCommand>()))
                .ThrowsAsync(new InvalidOperationException("boom"));

        var resp = await service.ProcessarAlunoRegistradoAsync(msg);

        resp.ValidationResult.IsValid.Should().BeFalse();
        resp.ValidationResult.Errors.Should().ContainSingle(e => e.PropertyName == "Exception" && e.ErrorMessage == "boom");

        VerifyLog(logger, LogLevel.Error, "Erro ao processar evento de usuário registrado", Times.Once());
    }
}
