namespace Core.Notification;

public interface INotificador
{
    bool TemErros();

    bool TemNotificacoes();

    void AdicionarErro(string mensagem);

    void Adicionar(TipoNotificacao tipo, string mensagem);

    List<string> ObterErros();

    List<string> ObterInformacoes();
}

public enum TipoNotificacao
{
    Informacao,
    Erro
}