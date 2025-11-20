namespace Core.DomainValidations;

public static class ValidacaoNumerica
{
    public static void DeveSerMaiorQueZero<T>(byte valor, string mensagem, ResultadoValidacao<T> resultado) where T : class
    {
        if (valor <= 0)
            resultado.AdicionarErro(mensagem);
    }

    public static void DeveSerMaiorQueZero<T>(short valor, string mensagem, ResultadoValidacao<T> resultado) where T : class
    {
        if (valor <= 0)
            resultado.AdicionarErro(mensagem);
    }

    public static void DeveSerMaiorQueZero<T>(int valor, string mensagem, ResultadoValidacao<T> resultado) where T : class
    {
        if (valor <= 0)
            resultado.AdicionarErro(mensagem);
    }

    public static void DeveEstarEntre<T>(int valor, int tamanhoMinimo, int tamanhoMaximo, string mensagem, ResultadoValidacao<T> resultado) where T : class
    {
        if (valor < tamanhoMinimo || valor > tamanhoMaximo)
            resultado.AdicionarErro(mensagem);
    }

    public static void DeveEstarEntre<T>(decimal valor, decimal tamanhoMinimo, decimal tamanhoMaximo, string mensagem, ResultadoValidacao<T> resultado) where T : class
    {
        if (valor < tamanhoMinimo || valor > tamanhoMaximo)
            resultado.AdicionarErro(mensagem);
    }

    public static void DeveSerMaiorQueZero<T>(decimal valor, string mensagem, ResultadoValidacao<T> resultado) where T : class
    {
        if (valor <= 0)
            resultado.AdicionarErro(mensagem);
    }
}
