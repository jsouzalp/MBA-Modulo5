using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

/// <summary>
/// Resposta detalhada de uma chamada de API
/// </summary>
/// <typeparam name="T">Tipo dos dados da resposta</typeparam>
[ExcludeFromCodeCoverage]
public class ApiResponse<T>
{
    /// <summary>
    /// Indica se a requisição foi bem-sucedida
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Código de status HTTP da resposta
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Dados da resposta (quando IsSuccess = true)
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Conteúdo do erro (quando IsSuccess = false)
    /// </summary>
    public string? ErrorContent { get; set; }
}
