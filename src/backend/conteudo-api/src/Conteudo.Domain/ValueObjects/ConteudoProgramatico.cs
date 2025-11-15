namespace Conteudo.Domain.ValueObjects;

public class ConteudoProgramatico
{
    public string Resumo { get; private set; }
    public string Descricao { get; private set; }
    public string Objetivos { get; private set; }
    public string PreRequisitos { get; private set; }
    public string PublicoAlvo { get; private set; }
    public string Metodologia { get; private set; }
    public string Recursos { get; private set; }
    public string Avaliacao { get; private set; }
    public string Bibliografia { get; private set; }

    protected ConteudoProgramatico()
    { }

    public ConteudoProgramatico(
        string resumo,
        string descricao,
        string objetivos,
        string preRequisitos,
        string publicoAlvo,
        string metodologia,
        string recursos,
        string avaliacao,
        string bibliografia)
    {
        ValidarDados(resumo, descricao, objetivos);

        Resumo = resumo;
        Descricao = descricao;
        Objetivos = objetivos;
        PreRequisitos = preRequisitos ?? string.Empty;
        PublicoAlvo = publicoAlvo ?? string.Empty;
        Metodologia = metodologia ?? string.Empty;
        Recursos = recursos ?? string.Empty;
        Avaliacao = avaliacao ?? string.Empty;
        Bibliografia = bibliografia ?? string.Empty;
    }

    private static void ValidarDados(string resumo, string descricao, string objetivos)
    {
        if (string.IsNullOrWhiteSpace(resumo))
            throw new ArgumentException("Resumo é obrigatório", nameof(resumo));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição é obrigatória", nameof(descricao));

        if (string.IsNullOrWhiteSpace(objetivos))
            throw new ArgumentException("Objetivos são obrigatórios", nameof(objetivos));
    }

    public ConteudoProgramatico Atualizar(
        string resumo,
        string descricao,
        string objetivos,
        string preRequisitos,
        string publicoAlvo,
        string metodologia,
        string recursos,
        string avaliacao,
        string bibliografia)
    {
        return new ConteudoProgramatico(
            resumo,
            descricao,
            objetivos,
            preRequisitos,
            publicoAlvo,
            metodologia,
            recursos,
            avaliacao,
            bibliografia);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        var other = (ConteudoProgramatico)obj;
        return Resumo == other.Resumo &&
               Descricao == other.Descricao &&
               Objetivos == other.Objetivos &&
               PreRequisitos == other.PreRequisitos &&
               PublicoAlvo == other.PublicoAlvo &&
               Metodologia == other.Metodologia &&
               Recursos == other.Recursos &&
               Avaliacao == other.Avaliacao &&
               Bibliografia == other.Bibliografia;
    }

    public override int GetHashCode()
    {
        var hash1 = HashCode.Combine(Resumo, Descricao, Objetivos, PreRequisitos, PublicoAlvo, Metodologia, Recursos, Avaliacao);
        var hash2 = Bibliografia.GetHashCode();
        return HashCode.Combine(hash1, hash2);
    }
}
