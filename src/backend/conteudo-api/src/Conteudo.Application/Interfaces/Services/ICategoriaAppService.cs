using Conteudo.Application.DTOs;

namespace Conteudo.Application.Interfaces.Services;

public interface ICategoriaAppService
{
    Task<IEnumerable<CategoriaDto>> ObterTodasCategoriasAsync();

    Task<CategoriaDto?> ObterPorIdAsync(Guid id);
}
