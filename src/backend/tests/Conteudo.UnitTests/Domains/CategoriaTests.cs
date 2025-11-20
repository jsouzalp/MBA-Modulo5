using Conteudo.Domain.Entities;
using Plataforma.Educacao.Core.Exceptions;

namespace Conteudo.UnitTests.Domains;
public class CategoriaTests
{
    // ---------- Happy path ----------
    [Fact]
    public void Deve_criar_categoria_valida_e_aplicar_defaults()
    {
        var c = new CategoriaBuilder().Build();

        c.Id.Should().NotBeEmpty();
        c.Nome.Should().Be("Arquitetura e Design");
        c.Descricao.Should().Be("Cursos sobre DDD, SOLID, Clean Architecture, etc.");
        c.Cor.Should().Be("#3366FF");
        c.IconeUrl.Should().Be("");  // default
        c.Ordem.Should().Be(0);      // default
        c.IsAtiva.Should().BeTrue(); // default: ativa ao criar
        c.Cursos.Should().NotBeNull();
        c.Cursos.Count.Should().Be(0);
    }

    // ---------- Validações de construção ----------
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_falhar_quando_nome_vazio(string nome)
    {
        Action act = () => new CategoriaBuilder().ComNome(nome).Build();

        act.Should().Throw<DomainException>()
           .WithMessage("*Nome da categoria é obrigatório*");
    }

    [Fact]
    public void Deve_falhar_quando_nome_maior_que_100()
    {
        var nome = new string('x', 101);

        Action act = () => new CategoriaBuilder().ComNome(nome).Build();

        act.Should().Throw<DomainException>()
           .WithMessage("*Nome da categoria não pode ter mais de 100 caracteres*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_falhar_quando_descricao_vazia(string descricao)
    {
        Action act = () => new CategoriaBuilder().ComDescricao(descricao).Build();

        act.Should().Throw<DomainException>()
           .WithMessage("*Descrição da categoria é obrigatória*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_falhar_quando_cor_vazia(string cor)
    {
        Action act = () => new CategoriaBuilder().ComCor(cor).Build();

        act.Should().Throw<DomainException>()
           .WithMessage("*Cor da categoria é obrigatória*");
    }

    // ---------- AtualizarInformacoes ----------
    [Fact]
    public void AtualizarInformacoes_deve_substituir_campos()
    {
        var c = new CategoriaBuilder().Build();

        c.AtualizarInformacoes(
            nome: "Engenharia de Software",
            descricao: "Conteúdos sobre práticas modernas de engenharia",
            cor: "#FF6633",
            iconeUrl: "https://cdn.exemplo.com/icons/engsoft.svg",
            ordem: 5
        );

        c.Nome.Should().Be("Engenharia de Software");
        c.Descricao.Should().Be("Conteúdos sobre práticas modernas de engenharia");
        c.Cor.Should().Be("#FF6633");
        c.IconeUrl.Should().Be("https://cdn.exemplo.com/icons/engsoft.svg");
        c.Ordem.Should().Be(5);
    }

    [Theory]
    [InlineData("", "ok", "ok", "Nome da categoria é obrigatório")]
    [InlineData("ok", "", "ok", "Descrição da categoria é obrigatória")]
    [InlineData("ok", "ok", "", "Cor da categoria é obrigatória")]
    public void AtualizarInformacoes_deve_validar_campos(string nome, string desc, string cor, string msg)
    {
        var c = new CategoriaBuilder().Build();

        c.Invoking(x => x.AtualizarInformacoes(nome, desc, cor))
         .Should().Throw<DomainException>()
         .WithMessage($"*{msg}*");
    }

    [Fact]
    public void AtualizarInformacoes_deve_validar_limite_de_nome()
    {
        var c = new CategoriaBuilder().Build();
        var nome = new string('x', 101);

        c.Invoking(x => x.AtualizarInformacoes(nome, "desc", "#123456"))
         .Should().Throw<DomainException>()
         .WithMessage("*Nome da categoria não pode ter mais de 100 caracteres*");
    }

    // ---------- Cursos (coleção somente leitura) ----------
    [Fact]
    public void Cursos_deve_ser_colecao_somente_leitura_vazia_ao_criar()
    {
        var c = new CategoriaBuilder().Build();

        c.Cursos.Should().BeAssignableTo<System.Collections.Generic.IReadOnlyCollection<Curso>>();
        c.Cursos.Count.Should().Be(0);
        // Não há Add na interface IReadOnlyCollection; garantimos apenas contagem/imutabilidade externa
    }
}
