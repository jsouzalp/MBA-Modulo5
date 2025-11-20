using Core.Mediator;
using Core.Messages;
using Core.Messages.Integration;
using Pagamentos.Domain.Entities;
using Pagamentos.Domain.Enum;
using Pagamentos.Domain.Interfaces;
using Pagamentos.Domain.Models;

namespace Pagamentos.Domain.Services
{
    public class PagamentoService(IPagamentoCartaoCreditoFacade pagamentoCartaoCreditoFacade,
                            IPagamentoRepository pagamentoRepository,
                            IMediatorHandler mediatorHandler) : IPagamentoService
    {
        public async Task<Transacao> RealizarPagamento(PagamentoCurso pagamentoAnuidade)
        {
            var cobranca = new CobrancaCurso
            {
                Id = pagamentoAnuidade.CursoId,
                Valor = pagamentoAnuidade.Total
            };

            var pagamento = new Pagamento
            {
                Valor = pagamentoAnuidade.Total,
                NomeCartao = pagamentoAnuidade.NomeCartao,
                NumeroCartao = pagamentoAnuidade.NumeroCartao,
                ExpiracaoCartao = pagamentoAnuidade.ExpiracaoCartao,
                CvvCartao = pagamentoAnuidade.CvvCartao,
                CobrancaCursoId = pagamentoAnuidade.CursoId,
                AlunoId = pagamentoAnuidade.ClienteId
            };

            var transacao = pagamentoCartaoCreditoFacade.RealizarPagamento(cobranca, pagamento);

            if (transacao.StatusTransacao == StatusTransacao.Pago)
            {
                await mediatorHandler.PublicarEvento(new PagamentoRealizadoEvent(cobranca.Id, pagamentoAnuidade.ClienteId, transacao.PagamentoId, transacao.Id, cobranca.Valor));

                pagamento.Status = transacao.StatusTransacao.ToString();
                pagamento.Transacao = transacao;

                pagamentoRepository.Adicionar(pagamento);
                pagamentoRepository.AdicionarTransacao(transacao);

                await pagamentoRepository.UnitOfWork.Commit();
                return transacao;
            }

            await mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz("pagamento", "A operadora recusou o pagamento"));
            await mediatorHandler.PublicarEvento(new PagamentoRecusadoEvent(cobranca.Id, pagamentoAnuidade.ClienteId, transacao.PagamentoId, transacao.Id, cobranca.Valor));

            return transacao;
        }
    }
}
