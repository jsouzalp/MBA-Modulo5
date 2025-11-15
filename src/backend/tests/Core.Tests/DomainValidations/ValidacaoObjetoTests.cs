using Core.DomainValidations;

namespace Core.Tests.DomainValidations;
public class ValidacaoObjetoTests
{
    private class Dummy { }

    [Fact]
    public void DeveEstarInstanciado_deve_adicionar_erro_quando_null()
    {
        var r = new ResultadoValidacao<Dummy>();

        ValidacaoObjeto.DeveEstarInstanciado(null!, "objeto nulo", r);

        r.Invoking(x => x.DispararExcecaoDominioSeInvalido())
         .Should().Throw<Plataforma.Educacao.Core.Exceptions.DomainException>()
         .Which.Errors.Should().Contain(e => e.Contains("objeto nulo"));
    }

    [Fact]
    public void DeveEstarInstanciado_nao_deve_adicionar_erro_quando_instanciado()
    {
        var r = new ResultadoValidacao<Dummy>();

        ValidacaoObjeto.DeveEstarInstanciado(new object(), "objeto nulo", r);

        r.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().NotThrow();
    }
}
