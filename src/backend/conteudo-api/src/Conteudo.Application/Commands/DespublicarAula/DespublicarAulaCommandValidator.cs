using FluentValidation;

namespace Conteudo.Application.Commands.DespublicarAula
{
    public class DespublicarAulaCommandValidator : AbstractValidator<DespublicarAulaCommand>
    {
        public DespublicarAulaCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("ID da aula é obrigatório")
                .NotEqual(Guid.Empty).WithMessage("ID da aula inválido");
        }
    }
}
