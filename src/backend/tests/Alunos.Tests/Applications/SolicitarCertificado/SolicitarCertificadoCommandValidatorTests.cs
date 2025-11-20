using Alunos.Application.Commands.SolicitarCertificado;
using FluentAssertions;

namespace Alunos.Tests.Applications.SolicitarCertificado;
public class SolicitarCertificadoCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_ids_vazios()
    {
        var cmd = new SolicitarCertificadoCommand(Guid.Empty, Guid.Empty);
        var result = new SolicitarCertificadoCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_ids_ok()
    {
        var cmd = new SolicitarCertificadoCommand(Guid.NewGuid(), Guid.NewGuid());
        new SolicitarCertificadoCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
