using Core.Messages.Integration;

namespace Core.Tests.Messages.Integration;
public class AlunoRegistradoIntegrationEventTests
{
    [Fact]
    public void Construtor_deve_preencher_todas_as_props()
    {
        var id = Guid.NewGuid();
        var evt = new AlunoRegistradoIntegrationEvent(
            id, "Fulano", "f@e.com", "12345678909",
            new DateTime(1990, 1, 1), "11", "M",
            "SP", "SP", "01001000", "foto.png"
        );

        evt.Id.Should().Be(id);
        evt.Nome.Should().Be("Fulano");
        evt.Email.Should().Be("f@e.com");
        evt.Cpf.Should().Be("12345678909");
        evt.DataNascimento.Should().Be(new DateTime(1990, 1, 1));
        evt.Telefone.Should().Be("11");
        evt.Genero.Should().Be("M");
        evt.Cidade.Should().Be("SP");
        evt.Estado.Should().Be("SP");
        evt.Cep.Should().Be("01001000");
        evt.Foto.Should().Be("foto.png");

        // sanity: é um IntegrationEvent
        evt.Should().BeAssignableTo<IntegrationEvent>();
    }
}
