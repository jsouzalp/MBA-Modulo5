using BFF.API.Models.Request;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using BFF.Infrastructure.Services;
using Core.Communication;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace BFF.API.Services.Pagamentos;

[ExcludeFromCodeCoverage]
public class PagamentoService(IOptions<ApiSettings> apiSettings,
                        IApiClientService apiClient,
                        ILogger<PagamentoService> logger) : BaseApiService(apiClient, logger), IPagamentoService
{
    private readonly ApiSettings _apiSettings = apiSettings?.Value ?? throw new ArgumentNullException(nameof(apiSettings));
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<ResponseResult<bool>> ExecutarPagamento(PagamentoCursoInputModel pagamentoCursoInputModel)
    {
        if (pagamentoCursoInputModel is null)
        {
            return FailExecutarPagamento(400, "Payload de pagamento não pode ser nulo.");
        }

        _apiClient.SetBaseAddress(_apiSettings.PagamentosApiUrl);

        ApiResponse<ResponseResult<object>> apiResponse;

        try
        {
            apiResponse = await _apiClient.PostAsyncWithDetails<PagamentoCursoInputModel, ResponseResult<object>>("/api/v1/pagamentos/pagamento", pagamentoCursoInputModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao chamar Pagamentos API.");
            return FailExecutarPagamento(502, "Falha ao comunicar com a API de pagamentos.");
        }

        return apiResponse.IsSuccess ? Success() : MapApiError(apiResponse);
    }

    public async Task<ResponseResult<IEnumerable<PagamentoDto>>> ObterTodos()
    {
        _apiClient.SetBaseAddress(_apiSettings.PagamentosApiUrl);

        try
        {
            var result = await _apiClient.GetAsync<ResponseResult<ICollection<PagamentoDto>>>("/api/v1/pagamentos/obter_todos");

            if (result is null)
            {
                return FailEnumerable(502, "Sem resposta da API de pagamentos.");
            }

            if (result.Errors?.Mensagens is { Count: > 0 } msgs)
            {
                return FailEnumerable(result.Status == 0 ? 400 : result.Status, msgs.ToArray());
            }

            var data = (IEnumerable<PagamentoDto>)(result.Data ?? Array.Empty<PagamentoDto>());

            return new ResponseResult<IEnumerable<PagamentoDto>>
            {
                Status = result.Status == 0 ? 200 : result.Status,
                Data = data
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao chamar Pagamentos API (GET /obter_todos).");
            return FailEnumerable(502, "Falha ao comunicar com a API de pagamentos.");
        }
    }

    public async Task<ResponseResult<PagamentoDto>> ObterPorIdPagamento(Guid idPagamento)
    {
        _apiClient.SetBaseAddress(_apiSettings.PagamentosApiUrl);

        try
        {
            var result = await _apiClient
                .GetAsync<ResponseResult<PagamentoDto>>($"/api/v1/pagamentos/obter/{idPagamento}");

            if (result is null)
            {
                return FailObterPagamento(502, "Sem resposta da API de pagamentos.");
            }

            if (result.Errors?.Mensagens is { Count: > 0 } msgs)
            {
                return FailObterPagamento(result.Status == 0 ? 400 : result.Status, msgs.ToArray());
            }

            var data = result.Data ?? new PagamentoDto();

            return new ResponseResult<PagamentoDto>
            {
                Status = result.Status == 0 ? 200 : result.Status,
                Data = data
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao chamar Pagamentos API (GET /obter/{{IdPagamento}}). IdPagamento: {IdPagamento}", idPagamento);
            return FailObterPagamento(502, "Falha ao comunicar com a API de pagamentos.");
        }
    }

    private static ResponseResult<bool> Success() => new()
    {
        Status = 200,
        Data = true
    };

    private static ResponseResult<bool> FailExecutarPagamento(int status, params string[] mensagens) => new()
    {
        Status = status,
        Data = false,
        Errors = new ResponseErrorMessages { Mensagens = mensagens?.ToList() ?? new List<string>() }
    };

    private static ResponseResult<PagamentoDto> FailObterPagamento(int status, params string[] mensagens) => new()
    {
        Status = status,
        Data = new PagamentoDto(),
        Errors = new ResponseErrorMessages { Mensagens = mensagens?.ToList() ?? new List<string>() }
    };

    private ResponseResult<bool> MapApiError(ApiResponse<ResponseResult<object>> apiResponse)
    {
        if (apiResponse.Data is not null &&
            apiResponse.Data.Errors?.Mensagens is { Count: > 0 })
        {
            return FailExecutarPagamento(apiResponse.StatusCode, apiResponse.Data.Errors.Mensagens.ToArray());
        }

        var mensagens = ParseMensagens(apiResponse.ErrorContent);
        if (mensagens.Count > 0)
        {
            return FailExecutarPagamento(apiResponse.StatusCode, mensagens.ToArray());
        }

        return FailExecutarPagamento(apiResponse.StatusCode,
                    string.IsNullOrWhiteSpace(apiResponse.ErrorContent)
                        ? "Erro desconhecido na API."
                        : apiResponse.ErrorContent);
    }

    private static List<string> ParseMensagens(string? errorContent)
    {
        var msgs = new List<string>();
        if (string.IsNullOrWhiteSpace(errorContent)) return msgs;

        try
        {
            var rr = JsonSerializer.Deserialize<ResponseResult<object>>(errorContent, _jsonOptions);
            if (rr?.Errors?.Mensagens is { Count: > 0 })
            {
                msgs.AddRange(rr.Errors.Mensagens);
                return msgs;
            }
        }
        catch { }

        try
        {
            var em = JsonSerializer.Deserialize<ResponseErrorMessages>(errorContent, _jsonOptions);
            if (em?.Mensagens is { Count: > 0 })
            {
                msgs.AddRange(em.Mensagens);
                return msgs;
            }
        }
        catch { }

        msgs.Add(errorContent);
        return msgs;
    }

    private static ResponseResult<IEnumerable<PagamentoDto>> FailEnumerable(int status, params string[] mensagens) => new()
    {
        Status = status,
        Data = Enumerable.Empty<PagamentoDto>(),
        Errors = new ResponseErrorMessages
        {
            Mensagens = mensagens?.ToList() ?? new List<string>()
        }
    };
}
