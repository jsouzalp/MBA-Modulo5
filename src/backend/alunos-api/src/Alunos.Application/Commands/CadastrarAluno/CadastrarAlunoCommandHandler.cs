using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace Alunos.Application.Commands.CadastrarAluno;

public class CadastrarAlunoCommandHandler(IAlunoRepository alunoRepository, IMediatorHandler mediatorHandler) : IRequestHandler<CadastrarAlunoCommand, CommandResult>
{
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(CadastrarAlunoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _raizAgregacao = request.RaizAgregacao;
            if (!await ValidarRequisicao(request)) { return request.Resultado; }

            var aluno = new Aluno(request.Id,
                request.Nome,
                request.Email,
                request.Cpf,
                request.DataNascimento,
                request.Genero,
                request.Cidade,
                request.Estado,
                request.Cep,
                request.Foto);

            aluno.DefinirId(request.Id);
            aluno.AtivarAluno();
            await alunoRepository.AdicionarAsync(aluno);
            if (await alunoRepository.UnitOfWork.Commit()) { request.Resultado.Data = aluno.Id; }

            return request.Resultado;
        }
        catch (Exception ex)
        {
            request.Validacao.Errors.Add(new ValidationFailure("Exception", $"Erro ao registrar aluno: {ex.Message}"));
            return request.Resultado;
        }
    }

    private async Task<bool> ValidarRequisicao(CadastrarAlunoCommand request)
    {
        request.DefinirValidacao(new CadastrarAlunoCommandValidator().Validate(request));
        if (!request.EstaValido())
        {
            foreach (var erro in request.Erros)
            {
                await mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aluno), erro));
            }
            return false;
        }

        var alunoExistente = await alunoRepository.ObterPorIdAsync(request.Id);
        if (alunoExistente != null)
        {
            await mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aluno), "Já existe um aluno cadastrado com este código."));
            return false;
        }

        alunoExistente = await alunoRepository.ObterPorEmailAsync(request.Email);
        if (alunoExistente != null)
        {
            await mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aluno), "Já existe um aluno cadastrado com este email."));
            return false;
        }

        return true;
    }
}
