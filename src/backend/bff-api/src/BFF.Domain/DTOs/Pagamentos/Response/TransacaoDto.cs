using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs.Pagamentos.Response;

[ExcludeFromCodeCoverage]
public class TransacaoDto
{
    public Guid Id { get; set; }
    public string CodigoAutorizacao { get; set; } = string.Empty;
    public string BandeiraCartao { get; set; } = string.Empty;
    public string StatusTransacao { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
}
