using FluentValidation;

namespace Alunos.Application.Commands.RegistrarHistoricoAprendizado;

public class RegistrarHistoricoAprendizadoCommandValidator : AbstractValidator<RegistrarHistoricoAprendizadoCommand>
{
    public RegistrarHistoricoAprendizadoCommandValidator()
    {
        RuleFor(c => c.AlunoId).NotEqual(Guid.Empty).WithMessage("Id do aluno inválido.");
        RuleFor(c => c.MatriculaCursoId).NotEqual(Guid.Empty).WithMessage("Id da matrícula inválido.");
        RuleFor(c => c.AulaId).NotEqual(Guid.Empty).WithMessage("Id da aula inválido.");
        RuleFor(c => c.NomeAula).NotNull().WithMessage("Nome da aula precisa ser informada.")
            .MaximumLength(100).WithMessage("Nome da aula deve ter no máximo 100 caracteres");
        RuleFor(c => c.DuracaoMinutos).GreaterThan((byte)0).WithMessage("Duração da aula deve ser maior que zero.");
    }
}
