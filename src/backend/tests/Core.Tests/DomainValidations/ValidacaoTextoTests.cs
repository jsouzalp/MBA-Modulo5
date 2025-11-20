using Core.DomainValidations;

namespace Core.Tests.DomainValidations;
public class ValidacaoTextoTests
{
    private class Dummy { }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void DevePossuirConteudo_deve_falhar_em_branco(string? v)
    {
        var r = new ResultadoValidacao<Dummy>();
        ValidacaoTexto.DevePossuirConteudo(v!, "sem conteúdo", r);

        r.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().Throw<Exception>();
    }

    [Fact]
    public void DevePossuirTamanho_deve_respeitar_limites_e_tratar_min_zero_como_um()
    {
        var r1 = new ResultadoValidacao<Dummy>();
        ValidacaoTexto.DevePossuirTamanho("", 0, 10, "tamanho", r1); // min 0 vira 1
        r1.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().Throw<Exception>();

        var r2 = new ResultadoValidacao<Dummy>();
        ValidacaoTexto.DevePossuirTamanho("abc", 1, 3, "tamanho", r2);
        r2.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().NotThrow();
    }

    [Fact]
    public void DeveAtenderRegex_deve_validar_padroes()
    {
        var r1 = new ResultadoValidacao<Dummy>();
        ValidacaoTexto.DeveAtenderRegex("ABC", "^[A-Z]{3}$", "regex", r1); // ok
        r1.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().NotThrow();

        var r2 = new ResultadoValidacao<Dummy>();
        ValidacaoTexto.DeveAtenderRegex("AbC!", "^[A-Z]{3}$", "regex", r2); // falha
        r2.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().Throw<Exception>();
    }

    [Fact]
    public void DeveSerCpfValido_valida_apenas_tamanho_de_11_digitos()
    {
        var r1 = new ResultadoValidacao<Dummy>();
        ValidacaoTexto.DeveSerCpfValido("111.222.333-44", "cpf inválido", r1); // 11 dígitos
        r1.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().NotThrow();

        var r2 = new ResultadoValidacao<Dummy>();
        ValidacaoTexto.DeveSerCpfValido("123", "cpf inválido", r2);
        r2.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().Throw<Exception>();
    }

    [Fact]
    public void DeveSerDiferenteDe_atual_implementacao_adiciona_erro_quando_diferente()
    {
        // OBS: a implementação atual ADICIONA erro quando valor != comparação (parece invertido).
        var r = new ResultadoValidacao<Dummy>();
        ValidacaoTexto.DeveSerDiferenteDe("A", "B", "deveria ser diferente", r);

        r.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().Throw<Exception>();

        // Quando iguais, não adiciona erro:
        var r2 = new ResultadoValidacao<Dummy>();
        ValidacaoTexto.DeveSerDiferenteDe("A", "A", "deveria ser diferente", r2);
        r2.Invoking(x => x.DispararExcecaoDominioSeInvalido()).Should().NotThrow();
    }
}
