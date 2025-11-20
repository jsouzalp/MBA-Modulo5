using Pagamento.AntiCorruption.Interfaces;
using Pagamentos.Domain.Entities;
using Pagamentos.Domain.Enum;
using Pagamentos.Domain.Interfaces;
using Pagamentos.Domain.Models;

namespace Pagamento.AntiCorruption.Services
{
    public class PagamentoCartaoCreditoFacade(IPayPalGateway payPalGateway, IConfigurationManager configManager) : IPagamentoCartaoCreditoFacade
    {
        public Transacao RealizarPagamento(CobrancaCurso cobrancaCurso, Pagamentos.Domain.Entities.Pagamento pagamento)
        {
            var apiKey = configManager.GetValue("apiKey");
            var encriptionKey = configManager.GetValue("encriptionKey");

            var serviceKey = payPalGateway.GetPayPalServiceKey(apiKey, encriptionKey);
            var cardHashKey = payPalGateway.GetCardHashKey(serviceKey, pagamento.NumeroCartao);

            var pagamentoResult = payPalGateway.CommitTransaction(cardHashKey, cobrancaCurso.Id.ToString(), pagamento.Valor);

            var transacao = new Transacao
            {
                CobrancaCursoId = cobrancaCurso.Id,
                Total = cobrancaCurso.Valor,
                PagamentoId = pagamento.Id
            };

            if (pagamentoResult)
            {
                transacao.StatusTransacao = StatusTransacao.Pago;
                return transacao;
            }

            transacao.StatusTransacao = StatusTransacao.Recusado;
            return transacao;
        }
    }
}
