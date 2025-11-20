using Core.DomainObjects;
using Plataforma.Educacao.Core.Exceptions;

namespace Conteudo.Domain.Entities;

public class Categoria : Entidade, IRaizAgregacao
{
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public string Cor { get; private set; }
    public string IconeUrl { get; private set; }
    public bool IsAtiva { get; private set; }
    public int Ordem { get; private set; }

    private readonly List<Curso> _cursos = [];
    public IReadOnlyCollection<Curso> Cursos => _cursos.AsReadOnly();

    protected Categoria()
    { }

    public Categoria(
        string nome,
        string descricao,
        string cor,
        string iconeUrl = "",
        int ordem = 0)
    {
        ValidarDados(nome, descricao, cor);

        Nome = nome;
        Descricao = descricao;
        Cor = cor;
        IconeUrl = iconeUrl;
        Ordem = ordem;
        IsAtiva = true;
    }

    private static void ValidarDados(string nome, string descricao, string cor)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome da categoria é obrigatório");

        if (nome.Length > 100)
            throw new DomainException("Nome da categoria não pode ter mais de 100 caracteres");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição da categoria é obrigatória");

        if (string.IsNullOrWhiteSpace(cor))
            throw new DomainException("Cor da categoria é obrigatória");
    }

    public void AtualizarInformacoes(
        string nome,
        string descricao,
        string cor,
        string iconeUrl = "",
        int ordem = 0)
    {
        ValidarDados(nome, descricao, cor);

        Nome = nome;
        Descricao = descricao;
        Cor = cor;
        IconeUrl = iconeUrl;
        Ordem = ordem;

        AtualizarDataModificacao();
    }
}
