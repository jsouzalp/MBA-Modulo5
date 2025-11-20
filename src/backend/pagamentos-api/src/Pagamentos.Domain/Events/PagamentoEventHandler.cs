using Core.Messages.Integration;
using MediatR;
using Pagamentos.Domain.Interfaces;
using Pagamentos.Domain.Models;

namespace Pagamentos.Domain.Events
{
    public class PagamentoEventHandler(IPagamentoService pagamentoService) : INotificationHandler<PagamentoCursoEvent>
    {
        public async Task Handle(PagamentoCursoEvent message, CancellationToken cancellationToken)
        {
            var pagamentoCurso = new PagamentoCurso
            {
                CursoId = message.CursoId,
                ClienteId = message.AlunoId,
                Total = message.Total,
                NomeCartao = message.NomeCartao,
                NumeroCartao = message.NumeroCartao,
                ExpiracaoCartao = message.ExpiracaoCartao,
                CvvCartao = message.CvvCartao
            };

            await pagamentoService.RealizarPagamento(pagamentoCurso);
        }
    }
}
