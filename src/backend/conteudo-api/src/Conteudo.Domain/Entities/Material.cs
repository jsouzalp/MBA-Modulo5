using Core.DomainObjects;
using Plataforma.Educacao.Core.Exceptions;

namespace Conteudo.Domain.Entities;

public class Material : Entidade, IRaizAgregacao
{
    public Guid AulaId { get; private set; }
    public Aula Aula { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public string TipoMaterial { get; private set; }
    public string Url { get; private set; }
    public bool IsObrigatorio { get; private set; }
    public long TamanhoBytes { get; private set; }
    public string Extensao { get; private set; }
    public int Ordem { get; private set; }
    public bool IsAtivo { get; private set; }

    protected Material()
    { }

    public Material(
        Guid aulaId,
        string nome,
        string descricao,
        string tipoMaterial,
        string url,
        bool isObrigatorio = false,
        long tamanhoBytes = 0,
        string extensao = "",
        int ordem = 0)
    {
        ValidarDados(nome, descricao, tipoMaterial, url);

        AulaId = aulaId;
        Nome = nome;
        Descricao = descricao;
        TipoMaterial = tipoMaterial;
        Url = url;
        IsObrigatorio = isObrigatorio;
        TamanhoBytes = tamanhoBytes;
        Extensao = extensao;
        Ordem = ordem;
        IsAtivo = true;
    }

    private static void ValidarDados(string nome, string descricao, string tipoMaterial, string url)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do material é obrigatório");

        if (nome.Length > 200)
            throw new DomainException("Nome do material não pode ter mais de 200 caracteres");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição do material é obrigatória");

        if (string.IsNullOrWhiteSpace(tipoMaterial))
            throw new DomainException("Tipo do material é obrigatório");

        if (string.IsNullOrWhiteSpace(url))
            throw new DomainException("URL do material é obrigatória");
    }

    public void AtualizarInformacoes(
        string nome,
        string descricao,
        string tipoMaterial,
        string url,
        bool isObrigatorio,
        long tamanhoBytes = 0,
        string extensao = "",
        int ordem = 0)
    {
        ValidarDados(nome, descricao, tipoMaterial, url);

        Nome = nome;
        Descricao = descricao;
        TipoMaterial = tipoMaterial;
        Url = url;
        IsObrigatorio = isObrigatorio;
        TamanhoBytes = tamanhoBytes;
        Extensao = extensao;
        Ordem = ordem;

        AtualizarDataModificacao();
    }

    public void Ativar()
    {
        IsAtivo = true;
        AtualizarDataModificacao();
    }

    public void Desativar()
    {
        IsAtivo = false;
        AtualizarDataModificacao();
    }
}
