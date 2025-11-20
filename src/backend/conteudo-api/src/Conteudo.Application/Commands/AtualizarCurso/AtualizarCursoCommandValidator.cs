using FluentValidation;

namespace Conteudo.Application.Commands.AtualizarCurso;

public class AtualizarCursoCommandValidator : AbstractValidator<AtualizarCursoCommand>
{
    public AtualizarCursoCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("ID é obrigatório")
            .NotEqual(Guid.Empty).WithMessage("ID do curso inválido.");
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");
        RuleFor(c => c.Valor)
            .GreaterThanOrEqualTo(0).WithMessage("Valor deve ser maior ou igual a zero");
        RuleFor(c => c.DuracaoHoras)
            .GreaterThan(0).WithMessage("Duração deve ser maior que zero");
        RuleFor(c => c.Nivel)
            .NotEmpty().WithMessage("Nível é obrigatório")
            .MaximumLength(50).WithMessage("Nível deve ter no máximo 50 caracteres");
        RuleFor(c => c.Instrutor)
            .NotEmpty().WithMessage("Instrutor é obrigatório")
            .MaximumLength(100).WithMessage("Instrutor deve ter no máximo 100 caracteres");
        RuleFor(c => c.VagasMaximas)
            .GreaterThan(0).WithMessage("Número de vagas deve ser maior que zero");
        RuleFor(c => c.ImagemUrl)
            .MaximumLength(500).WithMessage("URL da imagem deve ter no máximo 500 caracteres");
        RuleFor(c => c.Resumo)
            .NotEmpty().WithMessage("Resumo é obrigatório");
        RuleFor(c => c.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória");
        RuleFor(c => c.Objetivos)
            .NotEmpty().WithMessage("Objetivos são obrigatórios");
    }
}
