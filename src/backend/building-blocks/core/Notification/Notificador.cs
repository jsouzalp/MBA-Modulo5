namespace Core.Notification;

public class Notificador : INotificador
{
    private readonly List<Notificacao> _notificacoes;

    public Notificador()
    {
        _notificacoes = [];
    }

    public bool TemErros()
    {
        return _notificacoes.Any(n => n.Tipo == TipoNotificacao.Erro);
    }

    public bool TemNotificacoes()
    {
        return _notificacoes.Any();
    }

    public void AdicionarErro(string mensagem)
    {
        Adicionar(TipoNotificacao.Erro, mensagem);
    }

    public void Adicionar(TipoNotificacao tipo, string mensagem)
    {
        _notificacoes.Add(new Notificacao { Tipo = tipo, Mensagem = mensagem });
    }

    public List<string> ObterErros()
    {
        return _notificacoes.Where(n => n.Tipo == TipoNotificacao.Erro).Select(n => n.Mensagem).ToList();
    }

    public List<string> ObterInformacoes()
    {
        return _notificacoes.Where(n => n.Tipo == TipoNotificacao.Informacao).Select(n => n.Mensagem).ToList();
    }
}