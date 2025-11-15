using Conteudo.Domain.Entities;
using Core.Data;

namespace Conteudo.Domain.Interfaces.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IEnumerable<Categoria>> ObterTodosAsync();

    Task<Categoria?> ObterPorIdAsync(Guid id, bool noTracking = true);

    void Adicionar(Categoria categoria);

    void Atualizar(Categoria categoria);

    Task<bool> ExistePorNome(string nome);
}
