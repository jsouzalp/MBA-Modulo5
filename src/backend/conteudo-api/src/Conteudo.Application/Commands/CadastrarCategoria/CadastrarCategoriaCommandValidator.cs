using FluentValidation;

namespace Conteudo.Application.Commands.CadastrarCategoria;

public class CadastrarCategoriaCommandValidator : AbstractValidator<CadastrarCategoriaCommand>
{
    public CadastrarCategoriaCommandValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");
        RuleFor(c => c.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória");
        RuleFor(c => c.Cor)
            .NotEmpty().WithMessage("Cor é obrigatória")
            .MaximumLength(100).WithMessage("Cor deve ter no máximo 100 caracteres");
        RuleFor(c => c.IconeUrl)
            .MaximumLength(500).WithMessage("URL do ícone deve ter no máximo 500 caracteres");
    }
}
