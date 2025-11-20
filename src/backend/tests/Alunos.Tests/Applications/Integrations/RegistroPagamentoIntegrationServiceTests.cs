using Alunos.Application.Commands.AtualizarPagamento;
using Alunos.Application.Integration;
using Core.Mediator;
using Core.Messages.Integration;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace Alunos.Tests.Applications.Integrations;
public class RegistroPagamentoIntegrationServiceTests
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
    public async Task Deve_enviar_comando_e_retornar_ResponseMessage_quando_valido()
    {
        var mediator = new Mock<IMediatorHandler>();
        var logger = new Mock<ILogger<RegistroPagamentoIntegrationService>>();
        var service = new RegistroPagamentoIntegrationService(mediator.Object, logger.Object);

        var msg = new PagamentoMatriculaCursoIntegrationEvent(Guid.NewGuid(), Guid.NewGuid());
        var vr = new ValidationResult(); // IsValid = true

        mediator.Setup(m => m.EnviarComando(It.Is<AtualizarPagamentoMatriculaCommand>(c =>
            c.AlunoId == msg.AlunoId && c.MatriculaCursoId == msg.CursoId)))
            .ReturnsAsync(vr);

        var resp = await service.ProcessarPagamentoMatriculaCursoAsync(msg);

        resp.Should().NotBeNull();
        resp.ValidationResult.Should().BeSameAs(vr);
        VerifyLog(logger, LogLevel.Warning, "Falha na validação", Times.Never());
    }

    [Fact]
    public async Task Deve_logar_warning_e_retornar_response_quando_invalido()
    {
        var mediator = new Mock<IMediatorHandler>();
        var logger = new Mock<ILogger<RegistroPagamentoIntegrationService>>();
        var service = new RegistroPagamentoIntegrationService(mediator.Object, logger.Object);

        var msg = new PagamentoMatriculaCursoIntegrationEvent(Guid.NewGuid(), Guid.NewGuid());
        var vr = new ValidationResult([new ValidationFailure("a", "b")]);

        mediator.Setup(m => m.EnviarComando(It.IsAny<AtualizarPagamentoMatriculaCommand>())).ReturnsAsync(vr);

        var resp = await service.ProcessarPagamentoMatriculaCursoAsync(msg);

        resp.ValidationResult.Should().BeSameAs(vr);
        VerifyLog(logger, LogLevel.Warning, "Falha na validação do comando de registro de pagamento", Times.Once());
    }

    [Fact]
    public async Task Deve_logar_error_e_retornar_ResponseMessage_com_exception_em_caso_de_erro()
    {
        var mediator = new Mock<IMediatorHandler>();
        var logger = new Mock<ILogger<RegistroPagamentoIntegrationService>>();
        var service = new RegistroPagamentoIntegrationService(mediator.Object, logger.Object);

        var msg = new PagamentoMatriculaCursoIntegrationEvent(Guid.NewGuid(), Guid.NewGuid());

        mediator.Setup(m => m.EnviarComando(It.IsAny<AtualizarPagamentoMatriculaCommand>()))
                .ThrowsAsync(new InvalidOperationException("boom"));

        var resp = await service.ProcessarPagamentoMatriculaCursoAsync(msg);

        resp.ValidationResult.IsValid.Should().BeFalse();
        resp.ValidationResult.Errors.Should().ContainSingle(e => e.PropertyName == "Exception" && e.ErrorMessage == "boom");

        VerifyLog(logger, LogLevel.Error, "Erro ao processar evento de pagamento de matrícula registrado", Times.Once());
    }
}
