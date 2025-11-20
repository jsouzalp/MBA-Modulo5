using Mapster;
using Pagamentos.Application.Interfaces;
using Pagamentos.Application.ViewModels;
using Pagamentos.Domain.Interfaces;

namespace Pagamentos.Application.Services
{
    public class PagamentoAppService(IPagamentoRepository pagamentoRepository) : IPagamentoConsultaAppService, IPagamentoComandoAppService
    {
        public async Task<PagamentoViewModel> ObterPorId(Guid id)
        {
            var pagamento = await pagamentoRepository.ObterPorId(id);
            return pagamento.Adapt<PagamentoViewModel>();
        }

        public async Task<IEnumerable<PagamentoViewModel>> ObterTodos()
        {
            var pagamentos = await pagamentoRepository.ObterTodos();

            return pagamentos.Adapt<IEnumerable<PagamentoViewModel>>();
        }

        public void Dispose()
        {
            pagamentoRepository?.Dispose();
        }
    }
}
