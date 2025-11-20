using Core.Communication;
using Core.Communication.Filters;
using Core.SharedDtos.Conteudo;

namespace Conteudo.Application.Interfaces.Services;

public interface ICursoQuery
{
    Task<PagedResult<CursoDto>> ObterTodosAsync(CursoFilter filter);

    Task<CursoDto?> ObterPorIdAsync(Guid id, bool includeAulas = false);

    Task<IEnumerable<CursoDto>> ObterPorCategoriaIdAsync(Guid categoriaId, bool includeAulas = false);
}
