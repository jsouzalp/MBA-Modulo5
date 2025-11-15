using Mapster;
using Pagamentos.Application.ViewModels;
using Pagamentos.Domain.Entities;

namespace Pagamentos.Application.Mappings
{
    public class PagamentosMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            ConfigurePagamentoMappings(config);
            ConfigureTransacaoMappings(config);
            ConfigureCollections(config);
        }

        private static void ConfigurePagamentoMappings(TypeAdapterConfig config)
        {
            config.NewConfig<Pagamento, PagamentoViewModel>()
                  .Map(d => d.Transacao, s => s.Transacao)
                  .TwoWays();
        }

        private static void ConfigureTransacaoMappings(TypeAdapterConfig config)
        {
            config.NewConfig<Transacao, TransacaoViewModel>()
                  .TwoWays()
                  .PreserveReference(true);
        }

        private static void ConfigureCollections(TypeAdapterConfig config)
        {
            config.ForType<IEnumerable<Pagamento>, IEnumerable<PagamentoViewModel>>();
            config.ForType<IEnumerable<Transacao>, IEnumerable<TransacaoViewModel>>();
        }
    }
}
