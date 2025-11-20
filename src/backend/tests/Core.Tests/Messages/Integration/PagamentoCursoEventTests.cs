using Core.Messages.Integration;

namespace Core.Tests.Messages.Integration;
public class PagamentoCursoEventTests
{
    [Fact]
    public void Construtor_deve_mapear_propriedades_e_AggregateID()
    {
        var cursoId = Guid.NewGuid();
        var alunoId = Guid.NewGuid();

        var evt = new PagamentoCursoEvent(
            cursoId, alunoId, 123.45m,
            "NOME", "4111111111111111", "12/30", "123"
        );

        evt.CursoId.Should().Be(cursoId);
        evt.AggregateID.Should().Be(cursoId); // importante
        evt.AlunoId.Should().Be(alunoId);
        evt.Total.Should().Be(123.45m);
        evt.NomeCartao.Should().Be("NOME");
        evt.NumeroCartao.Should().Be("4111111111111111");
        evt.ExpiracaoCartao.Should().Be("12/30");
        evt.CvvCartao.Should().Be("123");
    }
}
