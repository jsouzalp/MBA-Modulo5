using BFF.API.Models.Request;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using BFF.Infrastructure.Services;
using Core.Communication;
using Core.Communication.Filters;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Services.Conteudos;

[ExcludeFromCodeCoverage]
public partial class ConteudoService : BaseApiService, IConteudoService
{
    private readonly ApiSettings _apiSettings;

    public ConteudoService(IOptions<ApiSettings> apiSettings,
                            IApiClientService apiClient,
                            ILogger<ConteudoService> logger) : base(apiClient, logger)
    {
        _apiSettings = apiSettings.Value;
        _apiClient.SetBaseAddress(_apiSettings.ConteudoApiUrl);
    }

    public async Task<ResponseResult<CursoDto>> ObterCursoPorIdAsync(Guid cursoId, bool includeAulas = false)
    {
        var result = await ExecuteWithErrorHandling(() => ObterCursoPorId(cursoId, includeAulas),
            nameof(ObterCursoPorIdAsync),
            cursoId);

        return result ?? ReturnUnknowError<CursoDto>();
    }

    public async Task<ResponseResult<PagedResult<CursoDto>>> ObterTodosCursosAsync(CursoFilter filter)
    {
        var result = await ExecuteWithErrorHandling(() => ObterTodosCursos(filter),
            nameof(ObterTodosCursosAsync),
            Guid.NewGuid());

        return result ?? ReturnUnknowError<PagedResult<CursoDto>>();
    }

    public async Task<ResponseResult<ConteudoProgramaticoDto>> ObterConteudoProgramaticoPorCursoIdAsync(Guid cursoId, bool includeAulas = false)
    {
        var result = await ExecuteWithErrorHandling(() => ObterConteudoProgramaticoPorCursoId(cursoId, includeAulas),
            nameof(ObterConteudoProgramaticoPorCursoIdAsync),
            cursoId);

        return result ?? ReturnUnknowError<ConteudoProgramaticoDto>();
    }

    public async Task<ResponseResult<Guid?>> AdicionarCursoAsync(CursoCriarRequest curso)
    {
        var result = await ExecuteWithErrorHandling(() => AdicionarCurso(curso),
            nameof(AdicionarCursoAsync),
            Guid.NewGuid());

        return result ?? ReturnUnknowError<Guid?>();
    }

    public async Task<ResponseResult<bool>> AtualizarCursoAsync(Guid id, AtualizarCursoRequest curso)
    {
        var result = await ExecuteWithErrorHandling(() => AtualizarCurso(id, curso),
            nameof(AtualizarCursoAsync),
            id);

        return result ?? ReturnUnknowError<bool>();
    }

    public async Task<ResponseResult<bool?>> ExcluirCursoAsync(Guid cursoId)
    {
        var result = await ExecuteWithErrorHandling(() => ExcluirCurso(cursoId),
            nameof(ExcluirCursoAsync),
            cursoId);

        return result ?? ReturnUnknowError<bool?>();
    }

    public async Task<ResponseResult<Guid?>> AdicionarAulaAsync(Guid cursoId, AulaCriarRequest aula)
    {
        var result = await ExecuteWithErrorHandling(() => AdicionarAula(cursoId, aula),
            nameof(AdicionarAulaAsync),
            cursoId);

        return result ?? ReturnUnknowError<Guid?>();
    }

    public async Task<ResponseResult<bool>> AtualizarAulaAsync(Guid cursoId, AulaAtualizarRequest aula)
    {
        var result = await ExecuteWithErrorHandling(
            () => AtualizarAula(cursoId, aula.Id, aula),
            nameof(AtualizarAulaAsync),
            aula.Id);

        return result ?? ReturnUnknowError<bool>();
    }

    public async Task<ResponseResult<bool>> ExcluirAulaAsync(Guid cursoId, Guid aulaId)
    {
        var result = await ExecuteWithErrorHandling(
            () => ExcluirAula(cursoId, aulaId),
            nameof(ExcluirAulaAsync),
            aulaId);

        return result ?? ReturnUnknowError<bool>();
    }

    public async Task<ResponseResult<IEnumerable<CursoDto>>> ObterPorCategoriaIdAsync(Guid categoriaId, bool includeAulas = false)
    {
        var result = await ExecuteWithErrorHandling(() => ObterPorCategoriaId(categoriaId, includeAulas),
            nameof(ObterPorCategoriaIdAsync),
            categoriaId);

        return result ?? ReturnUnknowError<IEnumerable<CursoDto>>();
    }

    public async Task<ResponseResult<IEnumerable<CategoriaDto>>> ObterTodasCategoriasAsync()
    {
        var result = await ExecuteWithErrorHandling(() => ObterTodasCategorias(),
            nameof(ObterTodasCategoriasAsync),
            Guid.NewGuid());

        return result ?? ReturnUnknowError<IEnumerable<CategoriaDto>>();
    }
}
