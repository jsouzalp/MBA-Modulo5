using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Infrastructure.Data;
using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Conteudo.Infrastructure.Repositories;

public class CategoriaRepository(ConteudoDbContext dbContext) : ICategoriaRepository
{
    public IUnitOfWork UnitOfWork => dbContext;

    private readonly DbSet<Categoria> _categoria = dbContext.Set<Categoria>();

    public async Task<IEnumerable<Categoria>> ObterTodosAsync()
    {
        return await _categoria
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Categoria> ObterPorIdAsync(Guid id, bool noTracking = true)
    {
        var query = _categoria.AsQueryable();
        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        return await query
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ExistePorNome(string nome)
    {
        return await _categoria
            .AsNoTracking()
            .AnyAsync(c => EF.Functions.Like(c.Nome, nome));
    }

    public void Adicionar(Categoria categoria)
    {
        _categoria.Add(categoria);
    }

    public void Atualizar(Categoria categoria)
    {
        _categoria.Update(categoria);
    }

    public void Dispose()
    {
        dbContext?.Dispose();
    }
}
