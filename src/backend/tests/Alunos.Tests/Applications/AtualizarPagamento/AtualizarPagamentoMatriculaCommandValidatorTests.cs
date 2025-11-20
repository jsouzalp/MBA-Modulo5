using Alunos.Application.Commands.AtualizarPagamento;
using FluentAssertions;

namespace Alunos.Tests.Applications.AtualizarPagamento;
public class AtualizarPagamentoMatriculaCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_ids_vazios()
    {
        var cmd = new AtualizarPagamentoMatriculaCommand(Guid.Empty, Guid.Empty);
        var result = new AtualizarPagamentoMatriculaCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_ids_ok()
    {
        var cmd = new AtualizarPagamentoMatriculaCommand(Guid.NewGuid(), Guid.NewGuid());
        new AtualizarPagamentoMatriculaCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
