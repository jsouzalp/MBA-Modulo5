using BFF.Domain.DTOs.Alunos.Response;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class DashboardAlunoDto
{
    [JsonIgnore]
    public AlunoDto Aluno { get; set; } = new();

    public List<MatriculaCursoDto> Matriculas { get; set; } = new();
    public List<CertificadoDto> Certificados { get; set; } = new();
    public ProgressoGeralDto ProgressoGeral { get; set; } = new();

    [JsonIgnore]
    public List<CursoDto> CursosRecomendados { get; set; } = new();

    [JsonIgnore]
    public List<PagamentoDto> PagamentosRecentes { get; set; } = new();
}
