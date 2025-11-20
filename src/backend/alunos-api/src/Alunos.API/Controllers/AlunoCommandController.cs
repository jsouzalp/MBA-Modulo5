using Alunos.Application.Commands.ConcluirCurso;
using Alunos.Application.Commands.MatricularAluno;
using Alunos.Application.Commands.RegistrarHistoricoAprendizado;
using Alunos.Application.Commands.SolicitarCertificado;
using Alunos.Application.DTOs.Request;
using Alunos.Application.Interfaces;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using Core.Services.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Alunos.API.Controllers;

[ExcludeFromCodeCoverage]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public partial class AlunoController(IMediatorHandler mediator,
    IAlunoQueryService alunoQueryService,
    INotificationHandler<DomainNotificacaoRaiz> notifications,
    INotificador notificador) : MainController(mediator, notifications, notificador)
{

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
    public async Task<IActionResult> MatricularAluno(Guid alunoId, MatriculaCursoRequest dto)
    {
        if (!ModelState.IsValid) { return RespostaPadraoApi<CommandResult>(ModelState); }
        if (alunoId != dto.AlunoId) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do aluno não confere"); }

        var comando = new MatricularAlunoCommand(dto.AlunoId, dto.CursoId, dto.CursoDisponivel, dto.Nome, dto.Valor, dto.Observacao);
        return RespostaPadraoApi<Guid?>(await _mediatorHandler.ExecutarComando(comando));
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
    public async Task<IActionResult> RegistrarHistoricoAprendizado(Guid alunoId, RegistroHistoricoAprendizadoRequest dto)
    {
        if (!ModelState.IsValid) { return RespostaPadraoApi<CommandResult>(ModelState); }
        if (alunoId != dto.AlunoId) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do aluno não confere"); }

        var matriculaCurso = await alunoQueryService.ObterInformacaoMatriculaCursoAsync(dto.MatriculaCursoId);
        if (matriculaCurso == null) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "Matrícula não encontrada"); }

        var comando = new RegistrarHistoricoAprendizadoCommand(dto.AlunoId,
            dto.MatriculaCursoId,
            dto.AulaId,
            dto.NomeAula,
            dto.DuracaoMinutos,
            dto.DataTermino
        );

        return RespostaPadraoApi<bool>(await _mediatorHandler.ExecutarComando(comando));
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
    public async Task<IActionResult> ConcluirCurso(Guid alunoId, ConcluirCursoRequest dto)
    {
        if (!ModelState.IsValid) { return RespostaPadraoApi<CommandResult>(ModelState); }
        if (alunoId != dto.AlunoId) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do aluno não confere"); }
        if (dto.CursoDto == null) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "Curso desta matrícula não encontrada"); }

        var matriculaCurso = await alunoQueryService.ObterInformacaoMatriculaCursoAsync(dto.MatriculaCursoId);
        if (matriculaCurso == null) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "Matrícula não encontrada"); }

        var comando = new ConcluirCursoCommand(dto.AlunoId, dto.MatriculaCursoId, dto.CursoDto);
        return RespostaPadraoApi<bool>(await _mediatorHandler.ExecutarComando(comando));
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
    public async Task<IActionResult> SolicitarCertificado(Guid alunoId, SolicitaCertificadoRequest dto)
    {
        if (!ModelState.IsValid) { return RespostaPadraoApi<CommandResult>(ModelState); }
        if (alunoId != dto.AlunoId) { return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do aluno não confere"); }

        var comando = new SolicitarCertificadoCommand(dto.AlunoId, dto.MatriculaCursoId);
        return RespostaPadraoApi<Guid?>(await _mediatorHandler.ExecutarComando(comando));
    }
}
