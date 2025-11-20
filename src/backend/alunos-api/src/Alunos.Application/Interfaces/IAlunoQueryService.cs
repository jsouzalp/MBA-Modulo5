using Alunos.Application.DTOs.Response;

namespace Alunos.Application.Interfaces;

public interface IAlunoQueryService
{
    Task<AlunoDto> ObterAlunoPorIdAsync(Guid alunoId);

    Task<EvolucaoAlunoDto> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId);

    Task<IEnumerable<MatriculaCursoDto>> ObterMatriculasPorAlunoIdAsync(Guid alunoId);

    Task<MatriculaCursoDto> ObterInformacaoMatriculaCursoAsync(Guid matriculaCursoId);

    Task<CertificadoDto> ObterCertificadoPorMatriculaIdAsync(Guid matriculaCursoId);

    Task<IEnumerable<AulaCursoDto>> ObterAulasPorMatriculaIdAsync(Guid matriculaCursoId);

    Task<IEnumerable<CertificadosDto>> ObterCertificadosPorAlunoIdAsync(Guid alunoId);

    Task<MatriculaCursoDto> ObterMatriculaPorIdAsync(Guid matriculaId, Guid alunoId);
}
