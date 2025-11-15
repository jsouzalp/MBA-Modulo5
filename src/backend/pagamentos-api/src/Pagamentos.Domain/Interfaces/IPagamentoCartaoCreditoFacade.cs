using Pagamentos.Domain.Entities;
using Pagamentos.Domain.Models;

namespace Pagamentos.Domain.Interfaces
{
    public interface IPagamentoCartaoCreditoFacade
    {
        Transacao RealizarPagamento(CobrancaCurso cobrancaCurso, Pagamento pagamento);
    }
}
