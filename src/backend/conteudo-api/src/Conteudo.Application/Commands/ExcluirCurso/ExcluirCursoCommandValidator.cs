using FluentValidation;

namespace Conteudo.Application.Commands.ExcluirCurso;

public class ExcluirCursoCommandValidator : AbstractValidator<ExcluirCursoCommand>
{
    public ExcluirCursoCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("O ID do curso é obrigatório.")
            .NotEqual(Guid.Empty).WithMessage("ID do curso inválido.");
    }
}
