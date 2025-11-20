using Plataforma.Educacao.Core.Exceptions;

namespace Conteudo.UnitTests.Domains;
public class MaterialTests
{
    // ---------- Happy path ----------
    [Fact]
    public void Deve_criar_material_valido()
    {
        var m = new MaterialBuilder().Build();

        m.Id.Should().NotBeEmpty(); // vindo de Entidade
        m.AulaId.Should().NotBeEmpty();
        m.Nome.Should().Be("Slides da Aula 1");
        m.Descricao.Should().Be("Conjunto de slides introdutórios");
        m.TipoMaterial.Should().Be("PDF");
        m.Url.Should().Be("https://cdn.exemplo.com/materials/aula1.pdf");
        m.IsObrigatorio.Should().BeFalse();
        m.TamanhoBytes.Should().Be(1024 * 1024);
        m.Extensao.Should().Be(".pdf");
        m.Ordem.Should().Be(1);
        m.IsAtivo.Should().BeTrue();
    }

    // ---------- Validações de construção ----------
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_falhar_quando_nome_vazio(string nome)
    {
        Action act = () => new MaterialBuilder().ComNome(nome).Build();

        act.Should().Throw<DomainException>()
           .WithMessage("*Nome do material é obrigatório*");
    }

    [Fact]
    public void Deve_falhar_quando_nome_maior_que_200()
    {
        var nome = new string('x', 201);

        Action act = () => new MaterialBuilder().ComNome(nome).Build();

        act.Should().Throw<DomainException>()
           .WithMessage("*Nome do material não pode ter mais de 200 caracteres*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_falhar_quando_descricao_vazia(string desc)
    {
        Action act = () => new MaterialBuilder().ComDescricao(desc).Build();

        act.Should().Throw<DomainException>()
           .WithMessage("*Descrição do material é obrigatória*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_falhar_quando_tipo_vazio(string tipo)
    {
        Action act = () => new MaterialBuilder().ComTipo(tipo).Build();

        act.Should().Throw<DomainException>()
           .WithMessage("*Tipo do material é obrigatório*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_falhar_quando_url_vazia(string url)
    {
        Action act = () => new MaterialBuilder().ComUrl(url).Build();

        act.Should().Throw<DomainException>()
           .WithMessage("*URL do material é obrigatória*");
    }

    // ---------- Comportamento atual: AulaId vazio é aceito ----------
    [Fact]
    public void Permite_aulaId_vazio_comportamento_atual()
    {
        var m = new MaterialBuilder().ComAulaId(Guid.Empty).Build();
        m.AulaId.Should().Be(Guid.Empty);
    }

    // ---------- AtualizarInformacoes ----------
    [Fact]
    public void AtualizarInformacoes_deve_substituir_todos_os_campos_editaveis()
    {
        var m = new MaterialBuilder().Build();

        m.AtualizarInformacoes(
            nome: "PDF Consolidado",
            descricao: "Todos os slides em um único arquivo",
            tipoMaterial: "PDF",
            url: "https://cdn.exemplo.com/materials/pack.pdf",
            isObrigatorio: true,
            tamanhoBytes: 2_000_000,
            extensao: ".pdf",
            ordem: 2
        );

        m.Nome.Should().Be("PDF Consolidado");
        m.Descricao.Should().Be("Todos os slides em um único arquivo");
        m.TipoMaterial.Should().Be("PDF");
        m.Url.Should().Be("https://cdn.exemplo.com/materials/pack.pdf");
        m.IsObrigatorio.Should().BeTrue();
        m.TamanhoBytes.Should().Be(2_000_000);
        m.Extensao.Should().Be(".pdf");
        m.Ordem.Should().Be(2);
    }

    [Theory]
    [InlineData("", "ok", "ok", "ok", "Nome do material é obrigatório")]
    [InlineData("ok", "", "ok", "ok", "Descrição do material é obrigatória")]
    [InlineData("ok", "ok", "", "ok", "Tipo do material é obrigatório")]
    [InlineData("ok", "ok", "ok", "", "URL do material é obrigatória")]
    public void AtualizarInformacoes_deve_validar_campos_obrigatorios(
        string nome, string desc, string tipo, string url, string msgEsperada)
    {
        var m = new MaterialBuilder().Build();

        m.Invoking(x => x.AtualizarInformacoes(
                nome, desc, tipo, url, isObrigatorio: true,
                tamanhoBytes: 100, extensao: ".pdf", ordem: 1))
         .Should().Throw<DomainException>()
         .WithMessage($"*{msgEsperada}*");
    }

    [Fact]
    public void AtualizarInformacoes_deve_validar_limite_de_nome()
    {
        var m = new MaterialBuilder().Build();
        var nome = new string('x', 201);

        m.Invoking(x => x.AtualizarInformacoes(
                nome, "desc", "tipo", "url", true, 100, ".pdf", 1))
         .Should().Throw<DomainException>()
         .WithMessage("*Nome do material não pode ter mais de 200 caracteres*");
    }

    // ---------- Ativar / Desativar ----------
    [Fact]
    public void Ativar_e_desativar_devem_alterar_IsAtivo()
    {
        var m = new MaterialBuilder().Build();

        m.IsAtivo.Should().BeTrue();
        m.Desativar();
        m.IsAtivo.Should().BeFalse();
        m.Ativar();
        m.IsAtivo.Should().BeTrue();
    }
}
