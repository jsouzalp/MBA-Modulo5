using FluentValidation;

namespace Conteudo.Application.Commands.AtualizarCategoria;

public class AtualizarCategoriaCommandValidator : AbstractValidator<AtualizarCategoriaCommand>
{
    public AtualizarCategoriaCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("O ID da categoria é obrigatório.");
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("O nome da categoria é obrigatório.");
        RuleFor(c => c.Descricao)
            .NotEmpty().WithMessage("A descrição da categoria é obrigatória.");
        RuleFor(c => c.Cor)
            .NotEmpty().WithMessage("A cor da categoria é obrigatória.")
            .Matches(@"^#([0-9a-fA-F]{3}|[0-9a-fA-F]{6})$").WithMessage("A cor deve ser um valor hexadecimal válido.");
        RuleFor(c => c.IconeUrl)
            .MaximumLength(500).WithMessage("URL do ícone deve ter no máximo 500 caracteres");
        RuleFor(c => c.Ordem)
            .GreaterThanOrEqualTo(0).WithMessage("A ordem deve ser um número inteiro não negativo.");
    }
}
