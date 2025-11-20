using Plataforma.Educacao.Core.Exceptions;

namespace Core.DomainValidations;

public class ResultadoValidacao<T> where T : class
{
    private readonly IList<string> _erros;
    private bool _estaValido => _erros.Count == 0;

    public ResultadoValidacao()
    {
        _erros = [];
    }

    public void AdicionarErro(string mensagem)
    {
        if (!string.IsNullOrWhiteSpace(mensagem))
            _erros.Add($"({typeof(T).Name}) {mensagem}");
    }

    public void DispararExcecaoDominioSeInvalido()
    {
        if (!_estaValido) { throw new DomainException(_erros); }
    }
}
