using FluentValidation;

namespace Conteudo.Application.Commands.ExcluirAula
{
    public class ExcluirAulaCommandValidator : AbstractValidator<ExcluirAulaCommand>
    {
        public ExcluirAulaCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("O ID da aula é obrigatório.")
                .NotEqual(Guid.Empty).WithMessage("ID da aula inválido.");
        }
    }
}
