using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class ApiActionResult<T>
{
    public bool Success { get; set; }

    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public object? ErrorContent { get; set; }

    public static ApiActionResult<T> SuccessResult(T data, string message = "Operação realizada com sucesso")
    {
        return new ApiActionResult<T>
        {
            Success = true,
            StatusCode = 200,
            Message = message,
            Data = data
        };
    }

    public static ApiActionResult<T> ErrorResult(int statusCode, string message, object? errorContent = null)
    {
        return new ApiActionResult<T>
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            ErrorContent = errorContent
        };
    }
}
