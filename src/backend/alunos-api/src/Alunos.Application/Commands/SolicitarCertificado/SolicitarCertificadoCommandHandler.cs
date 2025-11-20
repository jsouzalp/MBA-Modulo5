using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Alunos.Application.Commands.SolicitarCertificado;

public class SolicitarCertificadoCommandHandler(IAlunoRepository alunoRepository,
    IMediatorHandler mediatorHandler) : IRequestHandler<SolicitarCertificadoCommand, CommandResult>
{
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(SolicitarCertificadoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicao(request)) { return request.Resultado; }
        if (!ObterAluno(request.AlunoId, out Domain.Entities.Aluno aluno)) { return request.Resultado; }
        if (!ObterMatriculaCurso(request.MatriculaCursoId, out MatriculaCurso matriculaCurso)) { return request.Resultado; }

        decimal notaFinal = matriculaCurso.CalcularMediaFinalCurso();
        string pathCertificado = $"Certificado_{request.AlunoId}_{request.MatriculaCursoId}.pdf";
        string nomeInstrutor = "Curso Online";

        aluno.RequisitarCertificadoConclusao(request.MatriculaCursoId, notaFinal, pathCertificado, nomeInstrutor);
        var certificado = aluno.ObterMatriculaCursoPeloId(request.MatriculaCursoId).Certificado;

        await alunoRepository.AdicionarCertificadoMatriculaCursoAsync(certificado);
        if (await alunoRepository.UnitOfWork.Commit())
        {
            request.Resultado.Data = certificado.Id;
        }

        return request.Resultado;
    }

    private bool ValidarRequisicao(SolicitarCertificadoCommand request)
    {
        request.DefinirValidacao(new SolicitarCertificadoCommandValidator().Validate(request));

        if (!request.EstaValido())
        {
            foreach (var erro in request.Erros)
            {
                mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), erro)).GetAwaiter().GetResult();
            }

            return false;
        }

        return true;
    }

    private bool ObterAluno(Guid alunoId, out Domain.Entities.Aluno aluno)
    {
        aluno = alunoRepository.ObterPorIdAsync(alunoId).Result;
        if (aluno == null)
        {
            mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Aluno não encontrado.")).GetAwaiter().GetResult();
            return false;
        }

        return true;
    }

    private bool ObterMatriculaCurso(Guid matriculaCursoId, out MatriculaCurso matriculaCurso)
    {
        matriculaCurso = alunoRepository.ObterMatriculaPorIdAsync(matriculaCursoId).Result;
        if (matriculaCurso == null)
        {
            mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Matricula do aluno não encontrado.")).GetAwaiter().GetResult();
            return false;
        }

        return true;
    }
}
