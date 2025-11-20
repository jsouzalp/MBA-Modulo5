using BFF.Application.Interfaces.Services;
using Core.Communication;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace BFF.Infrastructure.Services;

[ExcludeFromCodeCoverage]
public abstract class BaseApiService
{
    protected readonly IApiClientService _apiClient;
    protected readonly ILogger _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    protected BaseApiService(IApiClientService apiClient, ILogger logger)
    {
        _apiClient = apiClient;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };
    }

    protected void ConfigureAuthToken(string token)
    {
        _apiClient.ClearDefaultHeaders();
        _apiClient.AddDefaultHeader("Authorization", $"Bearer {token}");
        _apiClient.AddDefaultHeader("Accept", "application/json");
    }

    protected async Task<T> ExecuteWithErrorHandling<T>(Func<Task<T>> operation, string operationName, params object[] parameters)
    {
        try
        {
            var result = await operation();

            if (result == null)
            {
                _logger.LogWarning("{OperationName} retornou null. Parâmetros: {@Parameters}", operationName, parameters);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar {OperationName}. Parâmetros: {@Parameters}", operationName, parameters);
            return default;
        }
    }

    protected bool ValidateToken(string token, string operationName)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            _logger.LogWarning("Token inválido para operação {OperationName}", operationName);
            return false;
        }

        return true;
    }

    protected ResponseResult<T> CaptureRequestError<T>(string result, int statusResult)
    {
        if (!string.IsNullOrEmpty(result))
        {
            try
            {
                var errorResponse = System.Text.Json.JsonSerializer.Deserialize<ResponseResult<T>>(result, _jsonOptions);
                return errorResponse;
            }
            catch
            {
                return new ResponseResult<T>
                {
                    Status = statusResult,
                    Errors = new ResponseErrorMessages { Mensagens = [result] }
                };
            }
        }

        return new ResponseResult<T>
        {
            Status = statusResult,
            Errors = new ResponseErrorMessages { Mensagens = ["Erro desconhecido na API"] }
        };
    }

    protected ResponseResult<T> ReturnUnknowError<T>()
    {
        return new ResponseResult<T> { Status = 500, Errors = new ResponseErrorMessages { Mensagens = ["Erro interno do servidor"] } };
    }
}
