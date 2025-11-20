using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pagamentos.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Pagamentos.Infrastructure.Mappings
{
    [ExcludeFromCodeCoverage]
    public class TransacaoMapping : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.HasKey(c => c.Id);

            builder.ToTable("Transacoes");
        }
    }
}
