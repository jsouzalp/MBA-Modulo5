using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Infrastructure.Data;
using Core.Communication;
using Core.Communication.Filters;
using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Conteudo.Infrastructure.Repositories;

public class CursoRepository(ConteudoDbContext dbContext) : ICursoRepository
{
    private readonly DbSet<Curso> _curso = dbContext.Set<Curso>();
    public IUnitOfWork UnitOfWork => dbContext;

    public async Task<PagedResult<Curso>> ObterTodosAsync(CursoFilter filter)
    {
        var query = _curso.AsQueryable();

        if (filter.IncludeAulas)
            query = query.Include(c => c.Aulas);

        if (!string.IsNullOrEmpty(filter.Query))
            query = query.Where(c => c.Nome.Contains(filter.Query));

        if (filter.Ativos)
            query = query.Where(c => c.Ativo);

        var totalResults = await query.CountAsync();

        var cursos = await query
            .Include(c => c.Categoria)
            .OrderBy(c => c.Nome)
            .Skip(filter.PageSize * (filter.PageIndex - 1))
            .Take(filter.PageSize)
            .AsNoTracking()
            .ToListAsync();

        return new PagedResult<Curso>
        {
            Items = cursos,
            PageIndex = filter.PageIndex,
            PageSize = filter.PageSize,
            TotalResults = totalResults,
            Query = filter.Query
        };
    }

    public async Task<IEnumerable<Curso>> ObterTodosAsync(bool includeAulas = false)
    {
        var query = _curso.AsQueryable();

        if (includeAulas)
            query = query.Include(c => c.Aulas);

        return await query
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Curso> ObterPorIdAsync(Guid id, bool includeAulas = false, bool noTracking = true)
    {
        var query = _curso.AsQueryable();

        if (includeAulas)
            query = query.Include(c => c.Aulas);

        if (noTracking)
            query = query.AsNoTracking();

        return await query
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Curso>> ObterPorCategoriaIdAsync(Guid categoriaId, bool includeAulas = false)
    {
        var query = _curso
            .Where(c => c.CategoriaId == categoriaId)
            .AsNoTracking();

        if (includeAulas)
            query = query.Include(c => c.Aulas);

        return await query
            .Include(c => c.Categoria)
            .ToListAsync();
    }

    public async Task<bool> ExistePorNomeAsync(string nome, Guid? excludeId = null)
    {
        return await _curso.AnyAsync(c => c.Nome == nome && (excludeId == null || c.Id != excludeId));
    }

    public async Task Adicionar(Curso curso)
    {
        await _curso.AddAsync(curso);
    }

    public async Task Atualizar(Curso curso)
    {
        _curso.Update(curso);
        await Task.CompletedTask;
    }

    public async Task Deletar(Curso curso)
    {
        _curso.Remove(curso);
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        dbContext?.Dispose();
    }
}
