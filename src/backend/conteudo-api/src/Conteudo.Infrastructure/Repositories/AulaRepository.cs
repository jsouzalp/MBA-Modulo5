using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Infrastructure.Data;
using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Conteudo.Infrastructure.Repositories
{
    public class AulaRepository(ConteudoDbContext dbContext) : IAulaRepository
    {
        private readonly DbSet<Aula> _aula = dbContext.Set<Aula>();
        public IUnitOfWork UnitOfWork => dbContext;

        public async Task<IEnumerable<Aula>> ObterTodosAsync(Guid cursoId, bool includeMateriais = false)
        {
            var query = _aula.AsQueryable();

            if (includeMateriais)
                query = query.Include(a => a.Materiais);

            return await query
                .Where(a => a.CursoId == cursoId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Aula?> ObterPorIdAsync(Guid cursoId, Guid id, bool includeMateriais = false)
        {
            var query = _aula.AsQueryable();

            if (includeMateriais)
                query = query.Include(a => a.Materiais);

            return await query
                .FirstOrDefaultAsync(a => a.Id == id && a.CursoId == cursoId);
        }

        public async Task<IEnumerable<Aula>> ObterPorCursoIdAsync(Guid cursoId, bool includeMateriais = false)
        {
            var query = _aula
                .Where(a => a.CursoId == cursoId)
                .AsQueryable();

            if (includeMateriais)
                query = query.Include(a => a.Materiais);

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Aula>> ObterPublicadasAsync(bool includeMateriais = false)
        {
            var query = _aula
                .Where(a => a.IsPublicada)
                .AsQueryable();

            if (includeMateriais)
                query = query.Include(a => a.Materiais);

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Aula>> ObterPublicadasPorCursoIdAsync(Guid cursoId, bool includeMateriais = false)
        {
            var query = _aula
                .Where(a => a.CursoId == cursoId && a.IsPublicada)
                .AsQueryable();

            if (includeMateriais)
                query = query.Include(a => a.Materiais);

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExistePorNumeroAsync(Guid cursoId, int numero, Guid? excludeId = null)
        {
            return await _aula.AnyAsync(a =>
                a.CursoId == cursoId &&
                a.Numero == numero &&
                (excludeId == null || a.Id != excludeId));
        }

        public async Task<Aula> CadastrarAulaAsync(Aula aula)
        {
            await _aula.AddAsync(aula);
            return aula;
        }

        public async Task<Aula> AtualizarAulaAsync(Aula aula)
        {
            _aula.Update(aula);
            await Task.CompletedTask;
            return aula;
        }

        public async Task PublicarAulaAsync(Guid cursoId, Guid id)
        {
            var aula = await ObterPorIdAsync(cursoId, id);
            if (aula != null)
            {
                aula.Publicar();
                _aula.Update(aula);
                await Task.CompletedTask;
            }
        }

        public async Task DespublicarAulaAsync(Guid cursoId, Guid id)
        {
            var aula = await ObterPorIdAsync(cursoId, id);
            if (aula != null)
            {
                aula.Despublicar();
                _aula.Update(aula);
                await Task.CompletedTask;
            }
        }

        public async Task ExcluirAulaAsync(Guid cursoId, Guid id)
        {
            var aula = await ObterPorIdAsync(cursoId, id);
            if (aula != null)
            {
                _aula.Remove(aula);
                await Task.CompletedTask;
            }
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }
    }
}
