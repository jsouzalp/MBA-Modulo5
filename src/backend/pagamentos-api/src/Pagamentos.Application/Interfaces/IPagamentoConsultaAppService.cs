using Pagamentos.Application.ViewModels;

namespace Pagamentos.Application.Interfaces
{
    public interface IPagamentoConsultaAppService
    {
        Task<IEnumerable<PagamentoViewModel>> ObterTodos();

        Task<PagamentoViewModel> ObterPorId(Guid id);
    }
}
