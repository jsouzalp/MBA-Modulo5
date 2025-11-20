using BFF.Domain.DTOs;

namespace BFF.Application.Interfaces.Services;

public interface IApiClientService
{
    void SetBaseAddress(string baseAddress);

    void AddDefaultHeader(string key, string value);

    void ClearDefaultHeaders();

    Task<T> GetAsync<T>(string endpoint) where T : class;

    Task<ApiResponse<TResponse>> GetWithDetailsAsync<TResponse>(string endpoint)
        where TResponse : class;

    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        where TRequest : class
        where TResponse : class;

    Task<ApiResponse<TResponse>> PostAsyncWithDetails<TRequest, TResponse>(string endpoint, TRequest request)
        where TRequest : class
        where TResponse : class;

    Task<ApiActionResult<TResponse>> PostAsyncWithActionResult<TRequest, TResponse>(string endpoint, TRequest request, string successMessage = "Operação realizada com sucesso")
        where TRequest : class
        where TResponse : class;

    Task<ApiResponse<TResponse>> PutAsyncWithDetails<TRequest, TResponse>(string endpoint, TRequest request)
        where TRequest : class
        where TResponse : class;

    Task<ApiActionResult<TResponse>> PutAsyncWithActionResult<TRequest, TResponse>(string endpoint, TRequest request, string successMessage = "Operação realizada com sucesso")
        where TRequest : class
        where TResponse : class;

    Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest request)
        where TRequest : class
        where TResponse : class;

    Task<bool> DeleteAsync(string endpoint);
}