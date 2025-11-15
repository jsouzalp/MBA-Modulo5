using BFF.API.Extensions;
using BFF.API.Services.Aluno;
using BFF.Domain.DTOs.Alunos.Request;
using BFF.Domain.DTOs.Alunos.Response;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace BFF.API.Controllers;

/// <summary>
/// Controller de alunos no BFF - Orquestra chamadas para Alunos API
/// </summary>
[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlunosController(IAlunoService alunoService,
    IMediatorHandler mediator,
    INotificationHandler<DomainNotificacaoRaiz> notifications,
    INotificador notificador) : BffController(mediator, notifications, notificador)
{

    /// <summary>
    /// Obtem informações do Aluno e Matrículas vinculadas
    /// </summary>
    /// <param name="alunoId">ID do Aluno</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("{alunoId}")]
    [ProducesResponseType(typeof(ResponseResult<AlunoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterAlunoPorIdAsync(Guid alunoId)
    {
        if (alunoId == Guid.Empty)
        {
            return ProcessarErro(HttpStatusCode.BadRequest, "Id do aluno é inválido.");
        }

        var resultado = await alunoService.ObterAlunoPorIdAsync(alunoId);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Obtem a evolução do Aluno em um curso matriculado
    /// </summary>
    /// <param name="alunoId">ID do Aluno</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("{alunoId}/evolucao")]
    [ProducesResponseType(typeof(ResponseResult<EvolucaoAlunoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId)
    {
        if (alunoId == Guid.Empty)
        {
            return ProcessarErro(HttpStatusCode.BadRequest, "Id do aluno é inválido.");
        }

        var resultado = await alunoService.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(alunoId);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Obtem uma lista das matrículas do aluno logado
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("todas-matriculas")]
    [ProducesResponseType(typeof(ResponseResult<ICollection<MatriculaCursoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterMatriculasPorAlunoIdAsync()
    {
        var alunoId = User.GetUserId();
        if (alunoId == Guid.Empty)
        {
            return ProcessarErro(HttpStatusCode.BadRequest, "Usuário não autenticado.");
        }

        var resultado = await alunoService.ObterMatriculasPorAlunoIdAsync(alunoId);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    [Authorize(Roles = "Usuario")]
    [HttpGet("matricula/{matriculaId}")]
    [ProducesResponseType(typeof(ResponseResult<MatriculaCursoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterMatriculaPorIdAsync(Guid matriculaId)
    {
        if (matriculaId == Guid.Empty)
        {
            return ProcessarErro(HttpStatusCode.BadRequest, "Id da matrícula é inválida.");
        }
        var resultado = await alunoService.ObterMatriculaPorIdAsync(matriculaId);
        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }
        return BadRequest(resultado);
    }

    /// <summary>
    /// Obtem o certificado de conclusão de um curso
    /// </summary>
    /// <param name="matriculaId">ID da matrícula no curso</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("matricula/{matriculaId}/certificado")]
    [ProducesResponseType(typeof(ResponseResult<CertificadoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterCertificadoPorMatriculaIdAsync(Guid matriculaId)
    {
        if (matriculaId == Guid.Empty)
        {
            return ProcessarErro(HttpStatusCode.BadRequest, "Id da matrícula é inválida.");
        }

        var resultado = await alunoService.ObterCertificadoPorMatriculaIdAsync(matriculaId);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Obtem as aulas de um determinado curso onde o aluno está matriculado
    /// </summary>
    /// <param name="matriculaId">ID da matrícula</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("aulas/{matriculaId}")]
    [ProducesResponseType(typeof(ResponseResult<ICollection<AulaCursoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterAulasPorMatriculaIdAsync(Guid matriculaId)
    {
        if (matriculaId == Guid.Empty)
        {
            return ProcessarErro(HttpStatusCode.BadRequest, "Id da matrícula é inválida.");
        }

        var resultado = await alunoService.ObterAulasPorMatriculaIdAsync(matriculaId);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Realiza a matrícula do aluno em um curso
    /// </summary>
    /// <param name="alunoId">ID do aluno</param>
    /// <param name="dto">Objeto com informação do curso</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/matricular-aluno")]
    [ProducesResponseType(typeof(ResponseResult<Guid?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MatricularAlunoAsync(Guid alunoId, MatriculaCursoRequest dto)
    {
        if (alunoId == Guid.Empty) { return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do aluno é inválida."); }

        var resultado = await alunoService.MatricularAlunoAsync(dto);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Registra o histórico de uma aula em andamento
    /// </summary>
    /// <param name="alunoId">ID do aluno</param>
    /// <param name="dto">Objeto com informação da aula</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/registrar-historico-aprendizado")]
    [ProducesResponseType(typeof(ResponseResult<bool?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegistrarHistoricoAprendizadoAsync(Guid alunoId, RegistroHistoricoAprendizadoRequest dto)
    {
        if (alunoId == Guid.Empty) { return ProcessarErro(HttpStatusCode.BadRequest, "Id do aluno é inválida."); }

        var resultado = await alunoService.RegistrarHistoricoAprendizadoAsync(dto);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Registra a conclusão do curso
    /// </summary>
    /// <param name="alunoId">ID do aluno</param>
    /// <param name="dto">Objeto com informação do curso</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpPut("{alunoId}/concluir-curso")]
    [ProducesResponseType(typeof(ResponseResult<bool?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConcluirCursoAsync(Guid alunoId, ConcluirCursoRequest dto)
    {
        if (alunoId == Guid.Empty) { return ProcessarErro(HttpStatusCode.BadRequest, "Id do aluno é inválida."); }

        var resultado = await alunoService.ConcluirCursoAsync(dto);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Registra a solicitação de conclusão do curso
    /// </summary>
    /// <param name="alunoId">ID do aluno</param>
    /// <param name="dto">Objeto com informação do curso</param>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpPost("{alunoId}/solicitar-certificado")]
    [ProducesResponseType(typeof(ResponseResult<Guid?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SolicitarCertificadoAsync(Guid alunoId, SolicitaCertificadoRequest dto)
    {
        if (alunoId == Guid.Empty) { return ProcessarErro(HttpStatusCode.BadRequest, "Id do aluno é inválida."); }

        var resultado = await alunoService.SolicitarCertificadoAsync(dto);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }

    /// <summary>
    /// Obtem os certificados do aluno logado
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Usuario")]
    [HttpGet("certificados")]
    [ProducesResponseType(typeof(ResponseResult<ICollection<CertificadosDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterCertificadosAsync()
    {
        var alunoId = User.GetUserId();
        if (alunoId == Guid.Empty)
        {
            return ProcessarErro(HttpStatusCode.BadRequest, "Usuário não autenticado.");
        }

        var resultado = await alunoService.ObterCertificadosPorAlunoIdAsync(alunoId);

        if (resultado?.Status == (int)HttpStatusCode.OK)
        {
            return Ok(resultado);
        }

        return BadRequest(resultado);
    }
}
