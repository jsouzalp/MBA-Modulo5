using FluentValidation;

namespace Alunos.Application.Commands.ConcluirCurso;

public class ConcluirCursoCommandValidator : AbstractValidator<ConcluirCursoCommand>
{
    public ConcluirCursoCommandValidator()
    {
        RuleFor(c => c.AlunoId).NotEqual(Guid.Empty).WithMessage("Id do aluno inválido.");
        RuleFor(c => c.MatriculaCursoId).NotEqual(Guid.Empty).WithMessage("Id da matrícula inválido.");
        RuleFor(c => c.CursoDto).NotNull().WithMessage("Curso precisa ser informado");
    }
}
