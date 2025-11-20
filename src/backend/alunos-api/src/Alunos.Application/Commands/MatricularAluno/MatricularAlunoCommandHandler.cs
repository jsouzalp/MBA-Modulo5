using Alunos.Domain.Interfaces;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Alunos.Application.Commands.MatricularAluno;

public class MatricularAlunoCommandHandler(IAlunoRepository alunoRepository,
    IMediatorHandler mediatorHandler) : IRequestHandler<MatricularAlunoCommand, CommandResult>
{
    private Guid _raizAgregacao;

    public IAlunoRepository AlunoRepository => alunoRepository;

    public async Task<CommandResult> Handle(MatricularAlunoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicao(request)) { return request.Resultado; }
        if (!ObterAluno(request.AlunoId, out Domain.Entities.Aluno aluno)) { return request.Resultado; }

        var matricula = aluno.MatricularAlunoEmCurso(request.CursoId, request.NomeCurso, request.ValorCurso, request.Observacao);
        await AlunoRepository.AdicionarMatriculaCursoAsync(matricula);
        if (await alunoRepository.UnitOfWork.Commit())
        {
            request.Resultado.Data = matricula.Id;
        }

        return request.Resultado;
    }

    private bool ValidarRequisicao(MatricularAlunoCommand request)
    {
        request.DefinirValidacao(new MatricularAlunoCommandValidator().Validate(request));
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
        aluno = AlunoRepository.ObterPorIdAsync(alunoId).Result;
        if (aluno == null)
        {
            mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Aluno não encontrado.")).GetAwaiter().GetResult();
            return false;
        }

        return true;
    }
}
