using FluentValidation;

namespace Conteudo.Application.Commands.PublicarAula
{
    public class PublicarAulaCommandValidator : AbstractValidator<PublicarAulaCommand>
    {
        public PublicarAulaCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("ID da aula é obrigatório")
                .NotEqual(Guid.Empty).WithMessage("ID da aula inválido");
        }
    }
}
