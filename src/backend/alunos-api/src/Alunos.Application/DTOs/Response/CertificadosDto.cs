using System.Diagnostics.CodeAnalysis;

namespace Alunos.Application.DTOs.Response;

[ExcludeFromCodeCoverage]
public class CertificadosDto
{
    public Guid Id { get; set; }
    public string NomeCurso { get; set; }
    public string Codigo { get; set; }
    public DateTime DataEmissao { get; set; }
    public string Url { get; set; }
}
