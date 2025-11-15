using BFF.Domain.DTOs.Alunos.Request;
using BFF.Domain.DTOs.Alunos.Response;
using Core.Communication;

namespace BFF.API.Services.Aluno;

public partial class AlunoService
{
    private async Task<ResponseResult<AlunoDto>> ObterAlunoPorId(Guid alunoId)
    {
        var url = $"api/aluno/{alunoId}";
        var apiResponse = await _apiClient.GetWithDetailsAsync<ResponseResult<AlunoDto>>(url);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<AlunoDto>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<EvolucaoAlunoDto>> ObterEvolucaoMatriculasCursoDoAlunoPorId(Guid alunoId)
    {
        var url = $"api/aluno/{alunoId}/evolucao";
        var apiResponse = await _apiClient.GetWithDetailsAsync<ResponseResult<EvolucaoAlunoDto>>(url);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<EvolucaoAlunoDto>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<ICollection<MatriculaCursoDto>>> ObterMatriculasPorAlunoId(Guid alunoId)
    {
        var url = "api/aluno/todas-matriculas";
        var apiResponse = await _apiClient.GetWithDetailsAsync<ResponseResult<ICollection<MatriculaCursoDto>>>(url);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<ICollection<MatriculaCursoDto>>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<CertificadoDto>> ObterCertificadoPorMatriculaId(Guid matriculaId)
    {
        var url = $"api/aluno/matricula/{matriculaId}/certificado";
        var apiResponse = await _apiClient.GetWithDetailsAsync<ResponseResult<CertificadoDto>>(url);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<CertificadoDto>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<MatriculaCursoDto>> ObterMatriculaPorId(Guid matriculaId)
    {
        var url = $"api/aluno/matricula/{matriculaId}";
        var apiResponse = await _apiClient.GetWithDetailsAsync<ResponseResult<MatriculaCursoDto>>(url);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<MatriculaCursoDto>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<ICollection<AulaCursoDto>>> ObterAulasPorMatriculaId(Guid matriculaId)
    {
        var url = $"api/aluno/aulas/{matriculaId}";
        var apiResponse = await _apiClient.GetWithDetailsAsync<ResponseResult<ICollection<AulaCursoDto>>>(url);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<ICollection<AulaCursoDto>>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<Guid?>> MatricularAluno(Guid alunoId, MatriculaCursoApiRequest matriculaCursoApi)
    {
        var apiResponse = await _apiClient.PostAsyncWithDetails<MatriculaCursoApiRequest, ResponseResult<Guid?>>($"api/aluno/{alunoId}/matricular-aluno", matriculaCursoApi);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<Guid?>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<bool?>> RegistrarHistoricoAprendizado(Guid alunoId, RegistroHistoricoAprendizadoApiRequest historicoAprendizado)
    {
        var apiResponse = await _apiClient.PostAsyncWithDetails<RegistroHistoricoAprendizadoApiRequest, ResponseResult<bool?>>($"api/aluno/{alunoId}/registrar-historico-aprendizado", historicoAprendizado);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<bool?>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<bool?>> ConcluirCurso(Guid alunoId, ConcluirCursoApiRequest concluirCurso)
    {
        var apiResponse = await _apiClient.PutAsyncWithDetails<ConcluirCursoApiRequest, ResponseResult<bool?>>($"api/aluno/{alunoId}/concluir-curso", concluirCurso);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<bool?>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    public async Task<ResponseResult<Guid?>> SolicitarCertificado(SolicitaCertificadoRequest dto)
    {
        var apiResponse = await _apiClient.PostAsyncWithDetails<SolicitaCertificadoRequest, ResponseResult<Guid?>>($"api/aluno/{dto.AlunoId}/solicitar-certificado", dto);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<Guid?>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }

    private async Task<ResponseResult<ICollection<CertificadosDto>>> ObterCertificados(Guid alunoId)
    {
        var url = $"api/aluno/{alunoId}/certificados";
        var apiResponse = await _apiClient.GetWithDetailsAsync<ResponseResult<ICollection<CertificadosDto>>>(url);
        if (apiResponse.IsSuccess) { return apiResponse.Data; }
        else { return CaptureRequestError<ICollection<CertificadosDto>>(apiResponse.ErrorContent, apiResponse.StatusCode); }
    }
}
