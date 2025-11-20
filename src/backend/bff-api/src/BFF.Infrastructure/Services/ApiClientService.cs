using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using BFF.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace BFF.Infrastructure.Services;

[ExcludeFromCodeCoverage]
public class ApiClientService : IApiClientService, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ResilienceSettings _resilienceSettings;
    private readonly ILogger<ApiClientService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClientService(IHttpClientFactory httpClientFactory, IOptions<ResilienceSettings> resilienceOptions, ILogger<ApiClientService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _resilienceSettings = resilienceOptions.Value;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };
    }

    public void SetBaseAddress(string baseAddress)
    {
        if (Uri.TryCreate(baseAddress, UriKind.Absolute, out var uri))
        {
            _httpClient.BaseAddress = uri;
        }
        else
        {
            _logger.LogError("Endereço base inválido: {BaseAddress}", baseAddress);
            throw new ArgumentException("Endereço base inválido", nameof(baseAddress));
        }
    }

    public void AddDefaultHeader(string key, string value) => _httpClient.DefaultRequestHeaders.Add(key, value);

    public void ClearDefaultHeaders() => _httpClient.DefaultRequestHeaders.Clear();

    public async Task<T?> GetAsync<T>(string endpoint) where T : class
    {
        try
        {
            var apiResponse = await GetWithDetailsAsync<T>(endpoint);
            return apiResponse.IsSuccess ? apiResponse.Data : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição GET para {Endpoint}", endpoint);
            return null;
        }
    }

    public async Task<ApiResponse<TResponse>> GetWithDetailsAsync<TResponse>(string endpoint)
        where TResponse : class
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
                return new ApiResponse<TResponse>
                {
                    IsSuccess = true,
                    StatusCode = (int)response.StatusCode,
                    Data = result
                };
            }

            _logger.LogWarning("GET falhou para {Endpoint}. Status: {StatusCode}. Response: {ErrorContent}",
                endpoint, response.StatusCode, responseContent);

            return new ApiResponse<TResponse>
            {
                IsSuccess = false,
                StatusCode = (int)response.StatusCode,
                ErrorContent = responseContent
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição GET para {Endpoint}", endpoint);
            return new ApiResponse<TResponse>
            {
                IsSuccess = false,
                StatusCode = 500,
                ErrorContent = "Erro interno do cliente HTTP"
            };
        }
    }

    public async Task<ApiActionResult<TResponse>> PostAsyncWithActionResult<TRequest, TResponse>(string endpoint, TRequest request, string successMessage = "Operação realizada com sucesso")
        where TRequest : class
        where TResponse : class
    {
        try
        {
            var apiResponse = await PostAsyncWithDetails<TRequest, TResponse>(endpoint, request);

            if (apiResponse.IsSuccess && apiResponse.Data != null)
            {
                return ApiActionResult<TResponse>.SuccessResult(apiResponse.Data, successMessage);
            }

            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<object>(apiResponse.ErrorContent, _jsonOptions);
                    return ApiActionResult<TResponse>.ErrorResult(apiResponse.StatusCode, "Erro na API", errorResponse);
                }
                catch
                {
                    return ApiActionResult<TResponse>.ErrorResult(apiResponse.StatusCode, "Erro na API", apiResponse.ErrorContent);
                }
            }

            return ApiActionResult<TResponse>.ErrorResult(400, "Erro desconhecido na API");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar requisição POST para {Endpoint}", endpoint);
            return ApiActionResult<TResponse>.ErrorResult(500, "Erro interno do servidor");
        }
    }

    public async Task<ApiActionResult<TResponse>> PutAsyncWithActionResult<TRequest, TResponse>(
        string endpoint,
        TRequest request,
        string successMessage = "Operação realizada com sucesso")
        where TRequest : class
        where TResponse : class
    {
        try
        {
            var apiResponse = await PutAsyncWithDetails<TRequest, TResponse>(endpoint, request);

            if (apiResponse.IsSuccess && apiResponse.Data != null)
            {
                return ApiActionResult<TResponse>.SuccessResult(apiResponse.Data, successMessage);
            }

            if (!string.IsNullOrEmpty(apiResponse.ErrorContent))
            {
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<object>(apiResponse.ErrorContent, _jsonOptions);
                    return ApiActionResult<TResponse>.ErrorResult(apiResponse.StatusCode, "Erro na API", errorResponse);
                }
                catch
                {
                    return ApiActionResult<TResponse>.ErrorResult(apiResponse.StatusCode, "Erro na API", apiResponse.ErrorContent);
                }
            }

            return ApiActionResult<TResponse>.ErrorResult(400, "Erro desconhecido na API");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar requisição PUT para {Endpoint}", endpoint);
            return ApiActionResult<TResponse>.ErrorResult(500, "Erro interno do servidor");
        }
    }

    public async Task<ApiResponse<TResponse>> PostAsyncWithDetails<TRequest, TResponse>(string endpoint, TRequest request)
        where TRequest : class
        where TResponse : class
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
                return new ApiResponse<TResponse>
                {
                    IsSuccess = true,
                    StatusCode = (int)response.StatusCode,
                    Data = result
                };
            }

            _logger.LogWarning("POST falhou para {Endpoint}. Status: {StatusCode}. Response: {ErrorContent}",
                endpoint, response.StatusCode, responseContent);

            return new ApiResponse<TResponse>
            {
                IsSuccess = false,
                StatusCode = (int)response.StatusCode,
                ErrorContent = responseContent
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição POST para {Endpoint}", endpoint);
            return new ApiResponse<TResponse>
            {
                IsSuccess = false,
                StatusCode = 500,
                ErrorContent = "Erro interno do cliente HTTP"
            };
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var apiResponse = await PostAsyncWithDetails<TRequest, TResponse>(endpoint, request);
        return apiResponse.IsSuccess ? apiResponse.Data : null;
    }

    public async Task<ApiResponse<TResponse>> PutAsyncWithDetails<TRequest, TResponse>(string endpoint, TRequest request)
        where TRequest : class
        where TResponse : class
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
                return new ApiResponse<TResponse>
                {
                    IsSuccess = true,
                    StatusCode = (int)response.StatusCode,
                    Data = result
                };
            }

            _logger.LogWarning("PUT falhou para {Endpoint}. Status: {StatusCode}. Response: {ErrorContent}",
                endpoint, response.StatusCode, responseContent);

            return new ApiResponse<TResponse>
            {
                IsSuccess = false,
                StatusCode = (int)response.StatusCode,
                ErrorContent = responseContent
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição PUT para {Endpoint}", endpoint);
            return new ApiResponse<TResponse>
            {
                IsSuccess = false,
                StatusCode = 500,
                ErrorContent = "Erro interno do cliente HTTP"
            };
        }
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var apiResponse = await PutAsyncWithDetails<TRequest, TResponse>(endpoint, request);
        return apiResponse.IsSuccess ? apiResponse.Data : null;
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            _logger.LogWarning("DELETE falhou para {Endpoint}. Status: {StatusCode}", endpoint, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição DELETE para {Endpoint}", endpoint);
            return false;
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
