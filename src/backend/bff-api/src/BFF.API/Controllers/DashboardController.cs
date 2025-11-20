using BFF.API.Extensions;
using BFF.API.Services.Aluno;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Controllers;

/// <summary>
/// Controller de Dashboard no BFF - Agrega dados de múltiplas APIs
/// </summary>
[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController(
    IDashboardService dashboardService,
    IMediatorHandler mediator,
    INotificationHandler<DomainNotificacaoRaiz> notifications,
    INotificador notificador,
    IAlunoService alunoService) : BffController(mediator, notifications, notificador)
{
    /// <summary>
    /// Obter dashboard do aluno
    /// </summary>
    /// <returns>Dados do dashboard do aluno</returns>
    [HttpGet("aluno")]
    [Authorize(Roles = "Usuario")]
    public async Task<IActionResult> GetDashboardAluno()
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty)
        {
            return ProcessarErro(System.Net.HttpStatusCode.Unauthorized, "Token inválido");
        }

        var matriculasTask = alunoService.ObterMatriculasPorAlunoIdAsync(userId);
        var evolucaoTask = alunoService.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(userId);

        await Task.WhenAll(matriculasTask, evolucaoTask);

        var matriculasResponse = await matriculasTask;
        var evolucaoResponse = await evolucaoTask;

        var matriculas = matriculasResponse?.Data?.ToList() ?? new List<BFF.Domain.DTOs.Alunos.Response.MatriculaCursoDto>();
        var certificados = matriculas
            .Where(m => m.Certificado != null)
            .Select(m => m.Certificado)
            .ToList();

        var totalAulas = evolucaoResponse?.Data?.MatriculasCursos?.Sum(m => m.QuantidadeAulasNoCurso) ?? 0;
        var aulasRealizadas = evolucaoResponse?.Data?.MatriculasCursos?.Sum(m => m.QuantidadeAulasRealizadas) ?? 0;
        var cursosConcluidos = matriculas.Count(m => m.DataConclusao.HasValue || string.Equals(m.EstadoMatricula, "Concluido", StringComparison.OrdinalIgnoreCase));
        var percentualGeral = totalAulas > 0 ? Math.Round((decimal)aulasRealizadas / totalAulas * 100, 2) : 0m;

        var progressoGeral = new ProgressoGeralDto
        {
            CursosMatriculados = matriculas.Count,
            CursosConcluidos = cursosConcluidos,
            CertificadosEmitidos = certificados.Count(c => c.DataEmissao.HasValue),
            PercentualConcluidoGeral = percentualGeral,
            HorasEstudadas = 0
        };

        var dashboard = new DashboardAlunoDto
        {
            Matriculas = matriculas,
            Certificados = certificados,
            ProgressoGeral = progressoGeral
        };

        return RespostaPadraoApi<DashboardAlunoDto>(System.Net.HttpStatusCode.OK, dashboard, "Dashboard do aluno obtido com sucesso");
    }

    /// <summary>
    /// Obter dashboard do administrador
    /// </summary>
    /// <returns>Dados do dashboard do administrador</returns>
    [HttpGet("admin")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetDashboardAdmin()
    {
        var dashboard = await dashboardService.GetDashboardAdminAsync();

        if (dashboard != null)
        {
            return RespostaPadraoApi<DashboardAdminDto>(System.Net.HttpStatusCode.OK, dashboard, "Dashboard do administrador obtido com sucesso");
        }

        return ProcessarErro(System.Net.HttpStatusCode.NotFound, "Dashboard não encontrado");
    }
}
