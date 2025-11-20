using Conteudo.Domain.Entities;
using Core.Data;

namespace Conteudo.Domain.Interfaces.Repositories;

public interface IAulaRepository : IRepository<Aula>
{
    Task<IEnumerable<Aula>> ObterTodosAsync(Guid cursoId, bool includeMateriais = false);

    Task<Aula?> ObterPorIdAsync(Guid cursoId, Guid id, bool includeMateriais = false);

    Task<IEnumerable<Aula>> ObterPorCursoIdAsync(Guid cursoId, bool includeMateriais = false);

    Task<IEnumerable<Aula>> ObterPublicadasAsync(bool includeMateriais = false);

    Task<IEnumerable<Aula>> ObterPublicadasPorCursoIdAsync(Guid cursoId, bool includeMateriais = false);

    Task<bool> ExistePorNumeroAsync(Guid cursoId, int numero, Guid? excludeId = null);

    Task<Aula> CadastrarAulaAsync(Aula aula);

    Task<Aula> AtualizarAulaAsync(Aula aula);

    Task PublicarAulaAsync(Guid cursoId, Guid id);

    Task DespublicarAulaAsync(Guid cursoId, Guid id);

    Task ExcluirAulaAsync(Guid cursoId, Guid id);
}
