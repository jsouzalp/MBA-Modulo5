using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class DashboardAdminDto
{
    public EstatisticasAlunosDto EstatisticasAlunos { get; set; } = new();
    public EstatisticasCursosDto EstatisticasCursos { get; set; } = new();
    public RelatorioVendasDto RelatorioVendas { get; set; } = new();
    public EstatisticasUsuariosDto EstatisticasUsuarios { get; set; } = new();
    public List<CursoPopularDto> CursosPopulares { get; set; } = new();
    public List<VendaRecenteDto> VendasRecentes { get; set; } = new();
}
