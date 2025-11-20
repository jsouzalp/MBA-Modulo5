using FluentValidation;

namespace Alunos.Application.Commands.AtualizarPagamento;

public class AtualizarPagamentoMatriculaCommandValidator : AbstractValidator<AtualizarPagamentoMatriculaCommand>
{
    public AtualizarPagamentoMatriculaCommandValidator()
    {
        RuleFor(c => c.AlunoId).NotEqual(Guid.Empty).WithMessage("Id do aluno é inválido");
        RuleFor(c => c.MatriculaCursoId).NotEqual(Guid.Empty).WithMessage("Id da matrícula é inválida");
    }
}
