using Core.Data;
using Microsoft.EntityFrameworkCore;
using Pagamentos.Domain.Entities;
using Pagamentos.Domain.Interfaces;
using Pagamentos.Infrastructure.Context;

namespace Pagamentos.Infrastructure.Repositories
{
    public class PagamentoRepository(PagamentoContext context) : IPagamentoRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public void Adicionar(Pagamento pagamento)
        {
            context.Pagamentos.Add(pagamento);
        }

        public void AdicionarTransacao(Transacao transacao)
        {
            context.Transacoes.Add(transacao);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public async Task<Pagamento> ObterPorId(Guid id)
        {
            return await context.Pagamentos.Include(u => u.Transacao).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pagamento>> ObterTodos()
        {
            return await context.Pagamentos.Include(u => u.Transacao).AsNoTracking().ToListAsync();
        }
    }
}
