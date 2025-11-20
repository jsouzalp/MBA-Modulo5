using MediatR;

namespace BFF.IntegrationTests.TestTypes;

// Tipo simples para os testes
public class DomainNotificacaoRaiz : INotification
{
    public Guid RaizAgregacao { get; set; }
    public DateTime DataHora { get; set; }
    public Guid NotificacaoId { get; set; }
    public string Chave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;

    public DomainNotificacaoRaiz(Guid raizAgregacao, string key, string value)
    {
        RaizAgregacao = raizAgregacao;
        DataHora = DateTime.Now;
        NotificacaoId = Guid.NewGuid();
        Chave = key;
        Valor = value;
    }

    public DomainNotificacaoRaiz(string key, string value)
    {
        DataHora = DateTime.Now;
        NotificacaoId = Guid.NewGuid();
        Chave = key;
        Valor = value;
    }
}

public interface INotificador
{
    void AdicionarErro(string mensagem);
}

public interface IMediatorHandler
{
    Task<TResponse> EnviarComando<TResponse>(IRequest<TResponse> comando);

    Task PublicarNotificacaoDominio<T>(T notificacao) where T : DomainNotificacaoRaiz;
}
