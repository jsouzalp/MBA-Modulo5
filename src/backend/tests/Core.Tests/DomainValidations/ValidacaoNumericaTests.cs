using Core.DomainValidations;

namespace Core.Tests.DomainValidations;
public class ValidacaoNumericaTests
{
    private class Dummy { }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void MaiorQueZero_int_deve_falhar_em_nao_positivos(int v)
    {
        var r = new ResultadoValidacao<Dummy>();
        ValidacaoNumerica.DeveSerMaiorQueZero(v, "int <= 0", r);

        r.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().Throw<Exception>();
    }

    [Fact]
    public void MaiorQueZero_decimal_deve_falhar_em_zero_ou_negativo_e_passar_em_positivo()
    {
        var r1 = new ResultadoValidacao<Dummy>();
        ValidacaoNumerica.DeveSerMaiorQueZero(0m, "dec <= 0", r1);
        r1.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().Throw<Exception>();

        var r2 = new ResultadoValidacao<Dummy>();
        ValidacaoNumerica.DeveSerMaiorQueZero(1m, "dec <= 0", r2);
        r2.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().NotThrow();
    }

    [Fact]
    public void DeveEstarEntre_int_deve_ser_inclusivo_nas_bordas()
    {
        var r = new ResultadoValidacao<Dummy>();
        ValidacaoNumerica.DeveEstarEntre(5, 5, 10, "fora", r);
        ValidacaoNumerica.DeveEstarEntre(10, 5, 10, "fora", r);

        r.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().NotThrow();
    }

    [Fact]
    public void DeveEstarEntre_decimal_deve_falhar_quando_for_abaixo_ou_acima()
    {
        var r = new ResultadoValidacao<Dummy>();
        ValidacaoNumerica.DeveEstarEntre(4.9m, 5m, 10m, "fora", r);
        ValidacaoNumerica.DeveEstarEntre(10.1m, 5m, 10m, "fora", r);

        r.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().Throw<Exception>();
    }
}
