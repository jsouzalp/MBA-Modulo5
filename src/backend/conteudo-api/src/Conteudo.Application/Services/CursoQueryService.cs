using Conteudo.Application.Interfaces.Services;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Communication.Filters;
using Core.SharedDtos.Conteudo;
using Mapster;

namespace Conteudo.Application.Services;

public class CursoQueryService(ICursoRepository cursoRepository) : ICursoQuery
{
    public async Task<PagedResult<CursoDto>> ObterTodosAsync(CursoFilter filter)
    {
        var cursos = await cursoRepository.ObterTodosAsync(filter);
        return cursos.Adapt<PagedResult<CursoDto>>();
    }

    public async Task<CursoDto?> ObterPorIdAsync(Guid id, bool includeAulas = false)
    {
        var curso = await cursoRepository.ObterPorIdAsync(id, includeAulas);
        return curso?.Adapt<CursoDto>();
    }

    public async Task<IEnumerable<CursoDto>> ObterPorCategoriaIdAsync(Guid categoriaId, bool includeAulas = false)
    {
        var cursos = await cursoRepository.ObterPorCategoriaIdAsync(categoriaId, includeAulas);
        return cursos.Adapt<IEnumerable<CursoDto>>();
    }
}
