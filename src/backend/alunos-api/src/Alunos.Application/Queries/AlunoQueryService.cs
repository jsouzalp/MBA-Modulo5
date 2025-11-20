using Alunos.Application.DTOs.Response;
using Alunos.Application.Interfaces;
using Alunos.Domain.Interfaces;
using Core.Utils;

namespace Alunos.Application.Queries;

public class AlunoQueryService(IAlunoRepository alunoRepository) : IAlunoQueryService
{
    public async Task<AlunoDto> ObterAlunoPorIdAsync(Guid alunoId)
    {
        var aluno = await alunoRepository.ObterPorIdAsync(alunoId);
        if (aluno == null) return null;

        return new AlunoDto
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            Email = aluno.Email,
            Cpf = aluno.Cpf,
            DataNascimento = aluno.DataNascimento,
            Telefone = aluno.Telefone,
            Ativo = aluno.Ativo,
            Genero = aluno.Genero,
            Cidade = aluno.Cidade,
            Estado = aluno.Estado,
            Cep = aluno.Cep,
            Foto = aluno.Foto,
            MatriculasCursos = aluno.MatriculasCursos != null ? aluno.MatriculasCursos.Select(m => new MatriculaCursoDto
            {
                Id = m.Id,
                CursoId = m.CursoId,
                NomeCurso = m.NomeCurso,
                AlunoId = m.AlunoId,
                PagamentoPodeSerRealizado = m.PagamentoPodeSerRealizado(),
                Valor = m.Valor,
                DataMatricula = m.DataMatricula,
                DataConclusao = m.DataConclusao,
                NotaFinal = m.ObterNotaFinalCurso(),
                Observacao = m.Observacao,
                EstadoMatricula = m.EstadoMatricula.ObterDescricao(),
                Certificado = m.Certificado != null ? new CertificadoDto
                {
                    Id = m.Certificado.Id,
                    MatriculaCursoId = m.Certificado.MatriculaCursoId,
                    NomeCurso = m.Certificado.NomeCurso,
                    DataSolicitacao = m.Certificado.DataSolicitacao,
                    DataEmissao = m.Certificado.DataEmissao,
                    CargaHoraria = m.Certificado.CargaHoraria,
                    NotaFinal = m.Certificado.NotaFinal,
                    PathCertificado = m.Certificado.PathCertificado,
                    NomeInstrutor = m.Certificado.NomeInstrutor
                } : null
            }).ToList() : []
        };
    }

    public async Task<EvolucaoAlunoDto> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId)
    {
        var aluno = await alunoRepository.ObterPorIdAsync(alunoId);
        if (aluno == null) return null;

        return new EvolucaoAlunoDto
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            Email = aluno.Email,
            DataNascimento = aluno.DataNascimento,
            MatriculasCursos = aluno.MatriculasCursos != null ? aluno.MatriculasCursos.Select(m => new EvolucaoMatriculaCursoDto
            {
                Id = m.Id,
                CursoId = m.CursoId,
                NomeCurso = m.NomeCurso,
                Valor = m.Valor,
                DataMatricula = m.DataMatricula,
                DataConclusao = m.DataConclusao,
                EstadoMatricula = m.EstadoMatricula.ObterDescricao(),
                QuantidadeAulasRealizadas = m.QuantidadeAulasFinalizadas(),
                QuantidadeAulasEmAndamento = m.QuantidadeAulasEmAndamento(),
                Certificado = m.Certificado != null ? new CertificadoDto
                {
                    Id = m.Certificado.Id,
                    DataSolicitacao = m.Certificado.DataSolicitacao,
                    PathCertificado = m.Certificado.PathCertificado,
                } : null
            }).ToList() : []
        };
    }

    public async Task<IEnumerable<MatriculaCursoDto>> ObterMatriculasPorAlunoIdAsync(Guid alunoId)
    {
        var aluno = await alunoRepository.ObterPorIdAsync(alunoId);
        if (aluno == null) return [];

        return aluno.MatriculasCursos.Select(m => new MatriculaCursoDto
        {
            Id = m.Id,
            CursoId = m.CursoId,
            NomeCurso = m.NomeCurso,
            AlunoId = m.AlunoId,
            PagamentoPodeSerRealizado = m.PagamentoPodeSerRealizado(),
            Valor = m.Valor,
            DataMatricula = m.DataMatricula,
            DataConclusao = m.DataConclusao,
            NotaFinal = m.ObterNotaFinalCurso(),
            Observacao = m.Observacao,
            EstadoMatricula = m.EstadoMatricula.ObterDescricao(),
            Certificado = m.Certificado != null ? new CertificadoDto
            {
                Id = m.Certificado.Id,
                MatriculaCursoId = m.Certificado.MatriculaCursoId,
                NomeCurso = m.Certificado.NomeCurso,
                DataSolicitacao = m.Certificado.DataSolicitacao,
                DataEmissao = m.Certificado.DataEmissao,
                CargaHoraria = m.Certificado.CargaHoraria,
                NotaFinal = m.Certificado.NotaFinal,
                PathCertificado = m.Certificado.PathCertificado,
                NomeInstrutor = m.Certificado.NomeInstrutor
            } : null
        });
    }

    public async Task<MatriculaCursoDto> ObterInformacaoMatriculaCursoAsync(Guid matriculaCursoId)
    {
        var matriculaCurso = await alunoRepository.ObterMatriculaPorIdAsync(matriculaCursoId);
        if (matriculaCurso == null) return null;

        return new MatriculaCursoDto
        {
            Id = matriculaCurso.Id,
            AlunoId = matriculaCurso.AlunoId,
            CursoId = matriculaCurso.CursoId,
            NomeCurso = matriculaCurso.NomeCurso,
            Valor = matriculaCurso.Valor,
            PagamentoPodeSerRealizado = matriculaCurso.PagamentoPodeSerRealizado(),
            DataMatricula = matriculaCurso.DataMatricula,
            DataConclusao = matriculaCurso.DataConclusao,
            NotaFinal = matriculaCurso.ObterNotaFinalCurso(),
            Observacao = matriculaCurso.Observacao,
            EstadoMatricula = matriculaCurso.EstadoMatricula.ObterDescricao(),
            Certificado = matriculaCurso.Certificado != null ? new CertificadoDto
            {
                Id = matriculaCurso.Certificado.Id,
                MatriculaCursoId = matriculaCurso.Certificado.MatriculaCursoId,
                NomeCurso = matriculaCurso.Certificado.NomeCurso,
                DataSolicitacao = matriculaCurso.Certificado.DataSolicitacao,
                DataEmissao = matriculaCurso.Certificado.DataEmissao,
                CargaHoraria = matriculaCurso.Certificado.CargaHoraria,
                NotaFinal = matriculaCurso.Certificado.NotaFinal,
                PathCertificado = matriculaCurso.Certificado.PathCertificado,
                NomeInstrutor = matriculaCurso.Certificado.NomeInstrutor
            } : null
        };
    }

    public async Task<CertificadoDto> ObterCertificadoPorMatriculaIdAsync(Guid matriculaCursoId)
    {
        var matricula = await alunoRepository.ObterMatriculaPorIdAsync(matriculaCursoId);
        if (matricula == null || matricula.Certificado == null) return null;

        return new CertificadoDto
        {
            Id = matricula.Certificado.Id,
            MatriculaCursoId = matricula.Certificado.MatriculaCursoId,
            NomeCurso = matricula.Certificado.NomeCurso,
            DataSolicitacao = matricula.Certificado.DataSolicitacao,
            DataEmissao = matricula.Certificado.DataEmissao,
            CargaHoraria = matricula.Certificado.CargaHoraria,
            NotaFinal = matricula.Certificado.NotaFinal,
            PathCertificado = matricula.Certificado.PathCertificado,
            NomeInstrutor = matricula.Certificado.NomeInstrutor
        };
    }

    public async Task<IEnumerable<AulaCursoDto>> ObterAulasPorMatriculaIdAsync(Guid matriculaCursoId)
    {
        var matricula = await alunoRepository.ObterMatriculaPorIdAsync(matriculaCursoId);
        if (matricula == null) return null;

        var retorno = new List<AulaCursoDto>();
        foreach (var aula in matricula.HistoricoAprendizado)
        {
            retorno.Add(new AulaCursoDto
            {
                AulaId = aula.AulaId,
                CursoId = aula.CursoId,
                NomeAula = aula.NomeAula,
                OrdemAula = 0,
                Ativo = false,
                DataInicio = aula.DataInicio,
                DataTermino = aula.DataTermino,
                Url = null
            });
        }

        return retorno.OrderBy(x => x.OrdemAula).ToList();
    }

    public async Task<IEnumerable<CertificadosDto>> ObterCertificadosPorAlunoIdAsync(Guid alunoId)
    {
        var aluno = await alunoRepository.ObterPorIdAsync(alunoId);
        if (aluno == null || aluno.MatriculasCursos == null) return [];

        var certificados = new List<CertificadosDto>();

        foreach (var matricula in aluno.MatriculasCursos)
        {
            if (matricula.Certificado != null && !string.IsNullOrEmpty(matricula.Certificado.PathCertificado))
            {
                certificados.Add(new CertificadosDto
                {
                    Id = matricula.Certificado.Id,
                    NomeCurso = matricula.Certificado.NomeCurso,
                    Codigo = matricula.Certificado.Id.ToString("N").Substring(0, 8).ToUpper(),
                    DataEmissao = matricula.Certificado.DataEmissao ?? matricula.Certificado.DataSolicitacao,
                    Url = matricula.Certificado.PathCertificado
                });
            }
        }

        return [.. certificados.OrderByDescending(x => x.DataEmissao)];
    }

    public async Task<MatriculaCursoDto?> ObterMatriculaPorIdAsync(Guid matriculaId, Guid alunoId)
    {
        var aluno = await alunoRepository.ObterPorIdAsync(alunoId);
        if (aluno == null) return null;

        var matricula = await alunoRepository.ObterMatriculaPorIdAsync(matriculaId);
        if (matricula == null || matricula.AlunoId != aluno.Id) return null;

        return new MatriculaCursoDto
        {
            Id = matricula.Id,
            AlunoId = matricula.AlunoId,
            CursoId = matricula.CursoId,
            NomeCurso = matricula.NomeCurso,
            Valor = matricula.Valor,
            PagamentoPodeSerRealizado = matricula.PagamentoPodeSerRealizado(),
            DataMatricula = matricula.DataMatricula,
            DataConclusao = matricula.DataConclusao,
            NotaFinal = matricula.ObterNotaFinalCurso(),
            Observacao = matricula.Observacao,
            EstadoMatricula = matricula.EstadoMatricula.ObterDescricao(),
            Certificado = matricula.Certificado != null ? new CertificadoDto
            {
                Id = matricula.Certificado.Id,
                MatriculaCursoId = matricula.Certificado.MatriculaCursoId,
                NomeCurso = matricula.Certificado.NomeCurso,
                DataSolicitacao = matricula.Certificado.DataSolicitacao,
                DataEmissao = matricula.Certificado.DataEmissao,
                CargaHoraria = matricula.Certificado.CargaHoraria,
                NotaFinal = matricula.Certificado.NotaFinal,
                PathCertificado = matricula.Certificado.PathCertificado,
                NomeInstrutor = matricula.Certificado.NomeInstrutor
            } : null
        };
    }
}
