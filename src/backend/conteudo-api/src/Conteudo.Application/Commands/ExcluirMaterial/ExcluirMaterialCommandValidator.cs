using FluentValidation;

namespace Conteudo.Application.Commands.ExcluirMaterial
{
    public class ExcluirMaterialCommandValidator : AbstractValidator<ExcluirMaterialCommand>
    {
        public ExcluirMaterialCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("O ID do material é obrigatório.")
                .NotEqual(Guid.Empty).WithMessage("ID do material inválido.");
        }
    }
}
