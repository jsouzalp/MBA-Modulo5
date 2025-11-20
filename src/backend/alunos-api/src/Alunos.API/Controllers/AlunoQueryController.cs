using Alunos.API.Extensions;
using Alunos.Application.DTOs.Response;
using Core.Communication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Alunos.API.Controllers;

public partial class AlunoController
{
    /// <summary>
    /// Obtem a informação do aluno
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseResult<AlunoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterAlunoPorId(Guid id)
    {
        var aluno = await alunoQueryService.ObterAlunoPorIdAsync(id);
        if (aluno == null)
        {
            _notificador.AdicionarErro("Aluno não encontrado.");
            return RespostaPadraoApi<string>();
        }

        return RespostaPadraoApi(data: aluno);
    }

    /// <summary>
    /// Obtem a evolução da matrícula do curso de um aluno
    /// </summary>
    /// <param name="alunoId">ID do aluno</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("{alunoId}/evolucao")]
    [ProducesResponseType(typeof(ResponseResult<EvolucaoAlunoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId)
    {
        var aluno = await alunoQueryService.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(alunoId);
        if (aluno == null)
        {
            _notificador.AdicionarErro("Evolução da matricula do aluno não encontrado.");
            return RespostaPadraoApi<string>();
        }

        return RespostaPadraoApi(data: aluno);
    }

    /// <summary>
    /// Obtem as informações de matrículas do aluno logado
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("todas-matriculas")]
    [ProducesResponseType(typeof(ResponseResult<ICollection<MatriculaCursoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterMatriculasPorAlunoId()
    {
        try
        {
            var alunoId = User.GetUserId();

            var matriculas = await alunoQueryService.ObterMatriculasPorAlunoIdAsync(alunoId);
            if (matriculas == null || !matriculas.Any())
            {
                _notificador.AdicionarErro("Matrícula do aluno não encontrada.");
                return RespostaPadraoApi<string>();
            }

            return RespostaPadraoApi(data: matriculas);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    [Authorize(Roles = "Usuario")]
    [HttpGet("matricula/{matriculaId}")]
    [ProducesResponseType(typeof(ResponseResult<MatriculaCursoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterMatriculaPorId(Guid matriculaId)
    {
        var alunoId = User.GetUserId();
        var matricula = await alunoQueryService.ObterMatriculaPorIdAsync(matriculaId, alunoId);
        if (matricula == null)
        {
            _notificador.AdicionarErro("Matrícula do aluno não encontrada.");
            return RespostaPadraoApi<string>();
        }
        return RespostaPadraoApi(data: matricula);
    }

    /// <summary>
    /// Obtem o certificado de conclusão do curso
    /// </summary>
    /// <param name="matriculaId">ID da matrícula do curso</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("matricula/{matriculaId}/certificado")]
    [ProducesResponseType(typeof(ResponseResult<CertificadoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterCertificadoPorMatriculaId(Guid matriculaId)
    {
        var certificado = await alunoQueryService.ObterCertificadoPorMatriculaIdAsync(matriculaId);
        if (certificado == null)
        {
            _notificador.AdicionarErro("Certificado não encontrado.");
            return RespostaPadraoApi<string>();
        }

        return RespostaPadraoApi(data: certificado);
    }

    /// <summary>
    /// Obtem as aulas de uma matrícula
    /// </summary>
    /// <param name="matriculaId">ID da matrícula do curso</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("aulas/{matriculaId}")]
    [ProducesResponseType(typeof(ResponseResult<ICollection<AulaCursoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterAulasPorMatriculaId(Guid matriculaId)
    {
        var aulas = await alunoQueryService.ObterAulasPorMatriculaIdAsync(matriculaId);
        if (aulas == null)
        {
            _notificador.AdicionarErro("Aulas não encontradas não encontrado.");
            return RespostaPadraoApi<string>();
        }

        return RespostaPadraoApi(data: aulas);
    }

    /// <summary>
    /// Obtem os certificados de um aluno
    /// </summary>
    /// <param name="alunoId">ID do aluno</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("{alunoId}/certificados")]
    [ProducesResponseType(typeof(ResponseResult<ICollection<CertificadosDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterCertificadosPorAlunoId(Guid alunoId)
    {
        try
        {
            var certificados = await alunoQueryService.ObterCertificadosPorAlunoIdAsync(alunoId);
            if (certificados == null || !certificados.Any())
            {
                _notificador.AdicionarErro("Nenhum certificado encontrado para o aluno.");
                return RespostaPadraoApi<string>();
            }

            return RespostaPadraoApi(data: certificados);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }
}
