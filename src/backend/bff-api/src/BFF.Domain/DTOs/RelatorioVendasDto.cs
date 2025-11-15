using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class RelatorioVendasDto
{
    public decimal VendasHoje { get; set; }
    public decimal VendasSemana { get; set; }
    public decimal VendasMes { get; set; }
    public decimal VendasAno { get; set; }
    public decimal TicketMedio { get; set; }
    public int TotalTransacoes { get; set; }
    public decimal TaxaConversao { get; set; }
}
