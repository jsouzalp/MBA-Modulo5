using Core.Data;
using Pagamentos.Domain.Entities;

namespace Pagamentos.Domain.Interfaces
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        Task<IEnumerable<Pagamento>> ObterTodos();

        Task<Pagamento> ObterPorId(Guid id);

        void Adicionar(Pagamento pagamento);

        void AdicionarTransacao(Transacao transacao);
    }
}
