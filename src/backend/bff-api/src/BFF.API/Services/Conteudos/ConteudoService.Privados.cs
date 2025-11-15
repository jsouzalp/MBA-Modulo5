using BFF.API.Models.Request;
using BFF.Domain.DTOs;
using Core.Communication;
using Core.Communication.Filters;
using Microsoft.AspNetCore.WebUtilities;

namespace BFF.API.Services.Conteudos;

public partial class ConteudoService
{
    public async Task<ResponseResult<CursoDto>> ObterCursoPorId(Guid cursoId, bool includeAulas = false)
    {
        var url = includeAulas ? $"api/cursos/{cursoId}?includeAulas=true" : $"api/cursos/{cursoId}";
        var apiResponse = await _apiClient.GetWithDetailsAsync<ResponseResult<CursoDto>>(url);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<CursoDto>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<PagedResult<CursoDto>>> ObterTodosCursos(CursoFilter filter)
    {
        var queryParams = new Dictionary<string, string>
        {
            [nameof(CursoFilter.PageSize)] = filter.PageSize > 0 ? filter.PageSize.ToString() : null,
            [nameof(CursoFilter.PageIndex)] = filter.PageIndex > 0 ? filter.PageIndex.ToString() : null,
            [nameof(CursoFilter.Query)] = string.IsNullOrWhiteSpace(filter.Query) ? null : filter.Query,
            [nameof(CursoFilter.IncludeAulas)] = filter.IncludeAulas.ToString().ToLowerInvariant(),
            [nameof(CursoFilter.Ativos)] = filter.Ativos.ToString().ToLowerInvariant()
        }!;

        var filteredParams = queryParams
            .Where(p => p.Value is not null)
            .ToDictionary(p => p.Key, p => p.Value);

        var url = QueryHelpers.AddQueryString("api/cursos", filteredParams);
        var apiResponse = await _apiClient.GetWithDetailsAsync<ResponseResult<PagedResult<CursoDto>>>(url);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<PagedResult<CursoDto>>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<ConteudoProgramaticoDto>> ObterConteudoProgramaticoPorCursoId(Guid cursoId, bool includeAulas = false)
    {
        var url = $"api/cursos/{cursoId}/conteudo-programatico";
        var apiResponse = await _apiClient.GetWithDetailsAsync<ResponseResult<ConteudoProgramaticoDto>>(url);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<ConteudoProgramaticoDto>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<Guid?>> AdicionarCurso(CursoCriarRequest curso)
    {
        var url = "api/cursos";
        var apiResponse = await _apiClient.PostAsyncWithDetails<CursoCriarRequest, ResponseResult<Guid?>>(url, curso);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<Guid?>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<bool>> AtualizarCurso(Guid id, AtualizarCursoRequest curso)
    {
        var url = $"api/cursos/{id}";
        var apiResponse = await _apiClient.PutAsyncWithDetails<AtualizarCursoRequest, ResponseResult<bool>>(url, curso);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<bool>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<bool?>> ExcluirCurso(Guid cursoId)
    {
        var url = $"api/cursos/{cursoId}";
        var apiResponse = await _apiClient.DeleteAsync(url);
        if (apiResponse)
        {
            return new ResponseResult<bool?> { Status = 200, Data = true };
        }
        else
        {
            return new ResponseResult<bool?>
            {
                Status = 400,
                Errors = new ResponseErrorMessages { Mensagens = new List<string> { "Erro ao excluir o curso" } }
            };
        }
    }

    public async Task<ResponseResult<Guid?>> AdicionarAula(Guid cursoId, AulaCriarRequest aula)
    {
        var url = $"api/cursos/{cursoId}/aulas";
        var apiResponse = await _apiClient.PostAsyncWithDetails<AulaCriarRequest, ResponseResult<Guid?>>(url, aula);

        return apiResponse.IsSuccess
            ? apiResponse.Data
            : CaptureRequestError<Guid?>(apiResponse.ErrorContent, apiResponse.StatusCode);
    }

    public async Task<ResponseResult<bool>> AtualizarAula(Guid cursoId, Guid aulaId, AulaAtualizarRequest aula)
    {
        if (aulaId != aula.Id)
            return new ResponseResult<bool> { Status = 400, Errors = new() { Mensagens = new List<string> { "ID da aula não confere" } } };

        var url = $"api/cursos/{cursoId}/aulas/{aulaId}";
        var apiResponse = await _apiClient.PutAsyncWithDetails<AulaAtualizarRequest, ResponseResult<bool>>(url, aula);

        return apiResponse.IsSuccess
            ? apiResponse.Data
            : CaptureRequestError<bool>(apiResponse.ErrorContent, apiResponse.StatusCode);
    }

    public async Task<ResponseResult<bool>> ExcluirAula(Guid cursoId, Guid aulaId)
    {
        var url = $"api/cursos/{cursoId}/aulas/{aulaId}";
        var apiResponse = await _apiClient.DeleteAsync(url);
        return apiResponse
            ? new ResponseResult<bool> { Status = 200, Data = true }
            : new ResponseResult<bool> { Status = 400, Errors = new() { Mensagens = new List<string> { "Erro ao excluir aula" } } };
    }

    public async Task<ResponseResult<IEnumerable<CursoDto>>> ObterPorCategoriaId(Guid categoriaId, bool includeAulas = false)
    {
        var url = includeAulas ? $"api/cursos/categoria/{categoriaId}?includeAulas=true" : $"api/cursos/categoria/{categoriaId}";
        var apiResponse = await _apiClient.GetWithDetailsAsync<ResponseResult<IEnumerable<CursoDto>>>(url);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<IEnumerable<CursoDto>>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<IEnumerable<CategoriaDto>>> ObterTodasCategorias()
    {
        var url = $"api/categoria";
        var apiResponse = await _apiClient.GetWithDetailsAsync<ResponseResult<IEnumerable<CategoriaDto>>>(url);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<IEnumerable<CategoriaDto>>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }
}
