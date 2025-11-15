using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.SharedDtos.Conteudo;
using MediatR;

namespace Alunos.Application.Commands.ConcluirCurso;

public class ConcluirCursoCommandHandler(IAlunoRepository alunoRepository,
    IMediatorHandler mediatorHandler) : IRequestHandler<ConcluirCursoCommand, CommandResult>
{
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(ConcluirCursoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicao(request)) { return request.Resultado; }
        if (!ObterAluno(request.AlunoId, out Domain.Entities.Aluno aluno)) { return request.Resultado; }
        //var matriculaCurso = aluno.ObterMatriculaCurso(request.MatriculaCursoId);

        if (!ValidarSeMatriculaCursoPodeSerConcluido(aluno, request.CursoDto, request.MatriculaCursoId)) { return request.Resultado; }

        aluno.ConcluirCurso(request.MatriculaCursoId);

        await alunoRepository.AtualizarAsync(aluno);
        if (await alunoRepository.UnitOfWork.Commit()) { request.Resultado.Data = true; }

        return request.Resultado;
    }

    private bool ValidarRequisicao(ConcluirCursoCommand request)
    {
        request.DefinirValidacao(new ConcluirCursoCommandValidator().Validate(request));
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

    private bool ValidarSeMatriculaCursoPodeSerConcluido(Aluno aluno, CursoDto cursoDto, Guid matriculaCursoId)
    {
        bool retorno = true;
        int quantidadeAulasPendentes = aluno.ObterQuantidadeAulasPendenteMatriculaCurso(cursoDto.Id);
        if (quantidadeAulasPendentes > 0)
        {
            retorno = false;
            mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), $"Existe(m) {quantidadeAulasPendentes} aula(s) pendente(s) para este curso")).GetAwaiter().GetResult();
        }

        int totalAulasAtivos = cursoDto.Aulas.Count();
        int totalAulasMatricula = aluno.ObterQuantidadeAulasMatriculaCurso(matriculaCursoId);
        if (totalAulasMatricula < totalAulasAtivos)
        {
            retorno = false;
            mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), $"Curso não pode ser concluído. Exist(m) {totalAulasAtivos - totalAulasMatricula} aula(s) não iniciada(s)")).GetAwaiter().GetResult();
        }

        return retorno;
    }
}
