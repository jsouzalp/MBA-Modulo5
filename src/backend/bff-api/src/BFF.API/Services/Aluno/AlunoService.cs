using BFF.API.Services.Conteudos;
using BFF.API.Settings;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs.Alunos.Request;
using BFF.Domain.DTOs.Alunos.Response;
using BFF.Infrastructure.Services;
using Core.Communication;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Services.Aluno;

[ExcludeFromCodeCoverage]
public partial class AlunoService : BaseApiService, IAlunoService
{
    private readonly ApiSettings _apiSettings;
    private readonly IConteudoService _conteudoService;

    public AlunoService(IOptions<ApiSettings> apiSettings,
        IConteudoService conteudoService,
        IApiClientService apiClient,
        ILogger<AlunoService> logger) : base(apiClient, logger)
    {
        _apiSettings = apiSettings.Value;
        _conteudoService = conteudoService;

        _apiClient.SetBaseAddress(_apiSettings.AlunosApiUrl);
    }

    public async Task<ResponseResult<AlunoDto>> ObterAlunoPorIdAsync(Guid alunoId)
    {
        var result = await ExecuteWithErrorHandling(() => ObterAlunoPorId(alunoId),
            nameof(ObterAlunoPorIdAsync),
            alunoId);

        return result ?? ReturnUnknowError<AlunoDto>();
    }

    public async Task<ResponseResult<EvolucaoAlunoDto>> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId)
    {
        var result = await ExecuteWithErrorHandling(() => ObterEvolucaoMatriculasCursoDoAlunoPorId(alunoId),
            nameof(ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync),
            alunoId);

        return result ?? ReturnUnknowError<EvolucaoAlunoDto>();
    }

    public async Task<ResponseResult<ICollection<MatriculaCursoDto>>> ObterMatriculasPorAlunoIdAsync(Guid alunoId)
    {
        var result = await ExecuteWithErrorHandling(() => ObterMatriculasPorAlunoId(alunoId),
            nameof(ObterMatriculasPorAlunoIdAsync),
            alunoId);

        return result ?? ReturnUnknowError<ICollection<MatriculaCursoDto>>();
    }

    public async Task<ResponseResult<CertificadoDto>> ObterCertificadoPorMatriculaIdAsync(Guid matriculaId)
    {
        var result = await ExecuteWithErrorHandling(() => ObterCertificadoPorMatriculaId(matriculaId),
            nameof(ObterCertificadoPorMatriculaIdAsync),
            matriculaId);

        return result ?? ReturnUnknowError<CertificadoDto>();
    }

    public async Task<ResponseResult<MatriculaCursoDto>> ObterMatriculaPorIdAsync(Guid matriculaId)
    {
        var result = await ExecuteWithErrorHandling(() => ObterMatriculaPorId(matriculaId),
            nameof(ObterMatriculaPorIdAsync),
            matriculaId);
        return result ?? ReturnUnknowError<MatriculaCursoDto>();
    }

    public async Task<ResponseResult<ICollection<AulaCursoDto>>> ObterAulasPorMatriculaIdAsync(Guid matriculaId)
    {
        var aulaCursoDto = await ExecuteWithErrorHandling(() => ObterAulasPorMatriculaId(matriculaId),
            nameof(ObterAulasPorMatriculaIdAsync),
            matriculaId);

        if (aulaCursoDto.Data != null)
        {
            var cursoMatriculado = await ObterMatriculaPorIdAsync(matriculaId);
            if (cursoMatriculado == null || cursoMatriculado.Data == null)
                return aulaCursoDto;

            var cursoDto = await _conteudoService.ObterCursoPorIdAsync(cursoMatriculado.Data.CursoId, includeAulas: true);

            foreach (var aula in cursoDto.Data.Aulas)
            {
                var historico = aulaCursoDto.Data.FirstOrDefault(h => h.AulaId == aula.Id);

                if (historico == null)
                {
                    aulaCursoDto.Data.Add(new AulaCursoDto
                    {
                        AulaId = aula.Id,
                        CursoId = aula.CursoId,
                        NomeAula = aula.Nome,
                        OrdemAula = aula.Numero,
                        DataInicio = null,
                        DataTermino = null,
                        AulaJaIniciadaRealizada = false,
                        Url = aula.VideoUrl
                    });
                }
            }
        }

        return aulaCursoDto ?? ReturnUnknowError<ICollection<AulaCursoDto>>();
    }

    public async Task<ResponseResult<Guid?>> MatricularAlunoAsync(MatriculaCursoRequest dto)
    {
        var cursoDto = await _conteudoService.ObterCursoPorIdAsync(dto.CursoId, includeAulas: false);
        if (cursoDto == null || cursoDto.Data == null)
        {
            return new ResponseResult<Guid?> { Status = 404, Errors = new ResponseErrorMessages { Mensagens = ["Curso não encontrado"] } };
        }

        var matriculaCursoApi = new MatriculaCursoApiRequest()
        {
            AlunoId = dto.AlunoId,
            CursoId = cursoDto.Data.Id,
            CursoDisponivel = cursoDto.Data.PodeSerMatriculado,
            Nome = cursoDto.Data.Nome,
            Valor = cursoDto.Data.Valor,
            Observacao = dto.Observacao
        };

        var result = await ExecuteWithErrorHandling(() => MatricularAluno(dto.AlunoId, matriculaCursoApi),
            nameof(MatricularAlunoAsync),
            dto.AlunoId);

        return result ?? ReturnUnknowError<Guid?>();
    }

    public async Task<ResponseResult<bool?>> RegistrarHistoricoAprendizadoAsync(RegistroHistoricoAprendizadoRequest dto)
    {
        var matriculaCurso = await ObterMatriculasPorAlunoIdAsync(dto.AlunoId);
        if (matriculaCurso == null || matriculaCurso.Data == null || !matriculaCurso.Data.Any(x => x.Id == dto.MatriculaCursoId))
        {
            return new ResponseResult<bool?> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Matrícula não encontrada"] } };
        }

        var cursoDto = await _conteudoService.ObterCursoPorIdAsync(matriculaCurso.Data.FirstOrDefault(x => x.Id == dto.MatriculaCursoId).CursoId, includeAulas: true);
        if (cursoDto == null || cursoDto.Data == null)
        {
            return new ResponseResult<bool?> { Status = 404, Errors = new ResponseErrorMessages { Mensagens = ["Curso não encontrado"] } };
        }

        if (!cursoDto.Data.Aulas.Any(x => x.Id == dto.AulaId))
        {
            return new ResponseResult<bool?> { Status = 404, Errors = new ResponseErrorMessages { Mensagens = ["Aula não encontrada"] } };
        }

        var historicoAprendizado = new RegistroHistoricoAprendizadoApiRequest
        {
            AlunoId = dto.AlunoId,
            MatriculaCursoId = dto.MatriculaCursoId,
            AulaId = dto.AulaId,
            NomeAula = cursoDto.Data.Nome,
            DuracaoMinutos = (byte)cursoDto.Data.DuracaoHoras,
            DataTermino = dto.DataTermino
        };

        var result = await ExecuteWithErrorHandling(() => RegistrarHistoricoAprendizado(dto.AlunoId, historicoAprendizado),
            nameof(RegistrarHistoricoAprendizadoAsync),
            dto.AlunoId);

        return result ?? ReturnUnknowError<bool?>();
    }

    public async Task<ResponseResult<bool?>> ConcluirCursoAsync(ConcluirCursoRequest dto)
    {
        var matriculaCurso = await ObterMatriculasPorAlunoIdAsync(dto.AlunoId);
        if (matriculaCurso == null || matriculaCurso.Data == null || !matriculaCurso.Data.Any(x => x.Id == dto.MatriculaCursoId))
        {
            return new ResponseResult<bool?> { Status = 400, Errors = new ResponseErrorMessages { Mensagens = ["Matrícula não encontrada"] } };
        }

        var cursoDto = await _conteudoService.ObterCursoPorIdAsync(matriculaCurso.Data.FirstOrDefault(x => x.Id == dto.MatriculaCursoId).CursoId, includeAulas: true);
        if (cursoDto == null || cursoDto.Data == null)
        {
            return new ResponseResult<bool?> { Status = 404, Errors = new ResponseErrorMessages { Mensagens = ["Curso não encontrado"] } };
        }

        var concluirCurso = new ConcluirCursoApiRequest
        {
            AlunoId = dto.AlunoId,
            MatriculaCursoId = dto.MatriculaCursoId,
            CursoDto = cursoDto.Data
        };

        var result = await ExecuteWithErrorHandling(() => ConcluirCurso(dto.AlunoId, concluirCurso),
            nameof(ConcluirCursoAsync),
            dto.AlunoId);

        return result ?? ReturnUnknowError<bool?>();
    }

    public async Task<ResponseResult<Guid?>> SolicitarCertificadoAsync(SolicitaCertificadoRequest dto)
    {
        var result = await ExecuteWithErrorHandling(() => SolicitarCertificado(dto),
            nameof(SolicitarCertificadoAsync),
            dto.AlunoId);

        return result ?? ReturnUnknowError<Guid?>();
    }

    public async Task<ResponseResult<ICollection<CertificadosDto>>> ObterCertificadosPorAlunoIdAsync(Guid alunoId)
    {
        var result = await ExecuteWithErrorHandling(() => ObterCertificados(alunoId),
            nameof(ObterCertificadosPorAlunoIdAsync),
            alunoId);

        return result ?? ReturnUnknowError<ICollection<CertificadosDto>>();
    }
}
