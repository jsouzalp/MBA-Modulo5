using FluentValidation;

namespace Conteudo.Application.Commands.CadastrarMaterial
{
    public class CadastrarMaterialCommandValidator : AbstractValidator<CadastrarMaterialCommand>
    {
        public CadastrarMaterialCommandValidator()
        {
            RuleFor(c => c.AulaId).NotEqual(Guid.Empty).WithMessage("ID da aula é obrigatório");
            RuleFor(c => c.Nome).NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");

            RuleFor(c => c.Descricao).NotEmpty().WithMessage("Descrição é obrigatória");

            RuleFor(c => c.TipoMaterial).NotEmpty().WithMessage("Tipo do material é obrigatório")
                .MaximumLength(50).WithMessage("Tipo do material deve ter no máximo 50 caracteres");

            RuleFor(c => c.Url).NotEmpty().WithMessage("URL é obrigatória")
                .MaximumLength(500).WithMessage("URL deve ter no máximo 500 caracteres");

            RuleFor(c => c.TamanhoBytes).GreaterThanOrEqualTo(0).WithMessage("Tamanho não pode ser negativo");

            RuleFor(c => c.Ordem).GreaterThanOrEqualTo(0).WithMessage("Ordem não pode ser negativa");
        }
    }
}
