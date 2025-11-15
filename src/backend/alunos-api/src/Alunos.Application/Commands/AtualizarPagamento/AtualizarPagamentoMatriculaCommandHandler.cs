using Alunos.Domain.Interfaces;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Alunos.Application.Commands.AtualizarPagamento;

public class AtualizarPagamentoMatriculaCommandHandler(IAlunoRepository alunoRepository,
    IMediatorHandler mediatorHandler) : IRequestHandler<AtualizarPagamentoMatriculaCommand, CommandResult>
{
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(AtualizarPagamentoMatriculaCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicao(request)) { return request.Resultado; }
        if (!ObterAluno(request.AlunoId, out Domain.Entities.Aluno aluno)) { return request.Resultado; }

        aluno.AtualizarPagamentoMatricula(request.MatriculaCursoId);

        await alunoRepository.AtualizarAsync(aluno);
        if (await alunoRepository.UnitOfWork.Commit()) { request.Resultado.Data = true; }

        return request.Resultado;
    }

    private bool ValidarRequisicao(AtualizarPagamentoMatriculaCommand request)
    {
        request.DefinirValidacao(new AtualizarPagamentoMatriculaCommandValidator().Validate(request));
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
}
