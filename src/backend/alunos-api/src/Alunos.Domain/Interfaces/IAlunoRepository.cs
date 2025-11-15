using Alunos.Domain.Entities;
using Alunos.Domain.ValueObjects;
using Core.Data;

namespace Alunos.Domain.Interfaces;

public interface IAlunoRepository : IRepository<Aluno>
{
    Task<Aluno> ObterPorIdAsync(Guid alunoId, bool noTracked = true);

    Task<Aluno> ObterPorEmailAsync(string email, bool noTracked = true);

    Task<Aluno> ObterPorCodigoUsuarioAsync(Guid codigoUsuario, bool noTracked = true);

    Task AdicionarAsync(Aluno aluno);

    Task AtualizarAsync(Aluno aluno);

    Task AdicionarMatriculaCursoAsync(MatriculaCurso matriculaCurso);

    Task AdicionarCertificadoMatriculaCursoAsync(Certificado certificado);

    Task<MatriculaCurso> ObterMatriculaPorIdAsync(Guid matriculaId, bool noTracked = true);

    //Task<MatriculaCurso> ObterMatriculaPorAlunoECursoAsync(Guid alunoId, Guid cursoId);

    Task AtualizarEstadoHistoricoAprendizadoAsync(HistoricoAprendizado historicoAntigo, HistoricoAprendizado historicoNovo);
}
