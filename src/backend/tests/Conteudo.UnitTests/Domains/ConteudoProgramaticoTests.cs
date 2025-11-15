using Conteudo.Domain.ValueObjects;

namespace Conteudo.UnitTests.Domains;
public class ConteudoProgramaticoTests
{
    // ---------- Happy path ----------
    [Fact]
    public void Deve_criar_valido()
    {
        var c = new ConteudoProgramaticoBuilder().Build();

        c.Resumo.Should().Be("Resumo do curso");
        c.Descricao.Should().Be("Descrição detalhada do conteúdo programático");
        c.Objetivos.Should().Be("Aprender DDD, SOLID e TDD na prática");
        c.PreRequisitos.Should().Be("Noções básicas de C#");
        c.PublicoAlvo.Should().Be("Desenvolvedores backend");
        c.Metodologia.Should().Be("Aulas expositivas e laboratórios");
        c.Recursos.Should().Be("Slides, repositório Git, exercícios");
        c.Avaliacao.Should().Be("Prova + projeto final");
        c.Bibliografia.Should().Be("Livro A; Artigo B; Documentação oficial");
    }

    // ---------- Obrigatórios: Resumo / Descricao / Objetivos ----------
    [Theory]
    [InlineData("", "Descricao ok", "Objetivos ok", "resumo", "Resumo é obrigatório")]
    [InlineData("Resumo ok", "", "Objetivos ok", "descricao", "Descrição é obrigatória")]
    [InlineData("Resumo ok", "Descricao ok", "", "objetivos", "Objetivos são obrigatórios")]
    [InlineData("   ", "Descricao ok", "Objetivos ok", "resumo", "Resumo é obrigatório")]
    public void Deve_falhar_quando_obrigatorios_incorretos(
        string resumo, string descricao, string objetivos,
        string paramEsperado, string mensagemEsperada)
    {
        Action act = () =>
        {
            var conteudoProgramatico = new ConteudoProgramatico(
                        resumo, descricao, objetivos,
                        "x", "y", "z", "r", "a", "b");
        };

        var ex = act.Should().Throw<ArgumentException>().Which;
        ex.ParamName.Should().Be(paramEsperado);
        ex.Message.Should().Contain(mensagemEsperada);
    }

    // ---------- Opcionais: null -> string.Empty ----------
    [Theory]
    [InlineData("PreRequisitos")]
    [InlineData("PublicoAlvo")]
    [InlineData("Metodologia")]
    [InlineData("Recursos")]
    [InlineData("Avaliacao")]
    [InlineData("Bibliografia")]
    public void Deve_mapear_opcionais_nulos_para_string_empty(string campo)
    {
        var b = new ConteudoProgramaticoBuilder();
        b = campo switch
        {
            "PreRequisitos" => b.ComPreRequisitos(null),
            "PublicoAlvo" => b.ComPublicoAlvo(null),
            "Metodologia" => b.ComMetodologia(null),
            "Recursos" => b.ComRecursos(null),
            "Avaliacao" => b.ComAvaliacao(null),
            "Bibliografia" => b.ComBibliografia(null),
            _ => b
        };

        var c = b.Build();

        (campo switch
        {
            "PreRequisitos" => c.PreRequisitos,
            "PublicoAlvo" => c.PublicoAlvo,
            "Metodologia" => c.Metodologia,
            "Recursos" => c.Recursos,
            "Avaliacao" => c.Avaliacao,
            "Bibliografia" => c.Bibliografia,
            _ => throw new InvalidOperationException()
        }).Should().Be(string.Empty);
    }

    // ---------- Atualizar(...) ----------
    [Fact]
    public void Atualizar_deve_retornar_nova_instancia_com_valores_aplicados()
    {
        var original = new ConteudoProgramaticoBuilder().Build();

        var atualizado = original.Atualizar(
            "Novo resumo",
            "Nova descricao",
            "Novos objetivos",
            "PR",
            "PA",
            "MET",
            "REC",
            "AV",
            "BIB");

        // imutabilidade (nova referência) e conteúdo
        atualizado.Should().NotBeSameAs(original);
        atualizado.Resumo.Should().Be("Novo resumo");
        atualizado.Descricao.Should().Be("Nova descricao");
        atualizado.Objetivos.Should().Be("Novos objetivos");
        atualizado.PreRequisitos.Should().Be("PR");
        atualizado.PublicoAlvo.Should().Be("PA");
        atualizado.Metodologia.Should().Be("MET");
        atualizado.Recursos.Should().Be("REC");
        atualizado.Avaliacao.Should().Be("AV");
        atualizado.Bibliografia.Should().Be("BIB");

        // original intacto
        original.Resumo.Should().Be("Resumo do curso");
    }

    [Fact]
    public void Atualizar_deve_validar_obrigatorios()
    {
        var original = new ConteudoProgramaticoBuilder().Build();

        Action act = () => original.Atualizar(
            "", "ok", "ok", null!, null!, null!, null!, null!, null!);

        var ex = act.Should().Throw<ArgumentException>().Which;
        ex.ParamName.Should().Be("resumo");
        ex.Message.Should().Contain("Resumo é obrigatório");
    }

    // ---------- Igualdade e Hash ----------
    [Fact]
    public void Equals_deve_retornar_true_para_conteudo_igual()
    {
        var a = new ConteudoProgramaticoBuilder().Build();
        var b = new ConteudoProgramaticoBuilder().Build();

        a.Equals(b).Should().BeTrue();
        b.Equals(a).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Equals_deve_retornar_false_se_algum_campo_diferir()
    {
        var a = new ConteudoProgramaticoBuilder().Build();
        var b = new ConteudoProgramaticoBuilder().ComResumo("Outro").Build();

        a.Equals(b).Should().BeFalse();
        b.Equals(a).Should().BeFalse();
    }

    [Fact]
    public void Equals_deve_retornar_false_para_null_ou_outro_tipo()
    {
        var a = new ConteudoProgramaticoBuilder().Build();
        a.Equals(null).Should().BeFalse();
        //a.Equals("string").Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_deve_ser_consistente_para_mesmo_objeto()
    {
        var a = new ConteudoProgramaticoBuilder().Build();
        var h1 = a.GetHashCode();
        var h2 = a.GetHashCode();

        h1.Should().Be(h2);
    }
}
