using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class EstatisticasUsuariosDto
{
    public int TotalUsuarios { get; set; }
    public int UsuariosAtivos { get; set; }
    public int UsuariosOnline { get; set; }
    public int AdminsAtivos { get; set; }
    public int AlunosAtivos { get; set; }
}
