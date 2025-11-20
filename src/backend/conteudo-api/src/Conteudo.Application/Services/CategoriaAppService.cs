using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Conteudo.Domain.Interfaces.Repositories;
using Mapster;

namespace Conteudo.Application.Services;

public class CategoriaAppService(ICategoriaRepository categoriaRepository) : ICategoriaAppService
{
    public async Task<IEnumerable<CategoriaDto>> ObterTodasCategoriasAsync()
    {
        var categorias = await categoriaRepository.ObterTodosAsync();
        return categorias.Adapt<IEnumerable<CategoriaDto>>();
    }

    public async Task<CategoriaDto?> ObterPorIdAsync(Guid id)
    {
        var categoria = await categoriaRepository.ObterPorIdAsync(id);
        return categoria?.Adapt<CategoriaDto>();
    }
}
