using FluentValidation;

namespace Conteudo.Application.Commands.CadastrarCurso;

public class CadastrarCursoCommandValidator : AbstractValidator<CadastrarCursoCommand>
{
    public CadastrarCursoCommandValidator()
    {
        // Curso
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");
        RuleFor(c => c.Valor)
            .NotEmpty().WithMessage("Valor é obrigatório")
            .GreaterThanOrEqualTo(0).WithMessage("Valor deve ser maior ou igual a zero");
        RuleFor(c => c.DuracaoHoras)
            .NotEmpty().WithMessage("Duração é obrigatória")
            .GreaterThan(0).WithMessage("Duração deve ser maior que zero");
        RuleFor(c => c.Nivel)
            .NotEmpty().WithMessage("Nível é obrigatório")
            .MaximumLength(50).WithMessage("Nível deve ter no máximo 50 caracteres");
        RuleFor(c => c.Instrutor)
            .NotEmpty().WithMessage("Instrutor é obrigatório")
            .MaximumLength(100).WithMessage("Instrutor deve ter no máximo 100 caracteres");
        RuleFor(c => c.VagasMaximas)
            .NotEmpty().WithMessage("Número máximo de vagas é obrigatório")
            .GreaterThan(0).WithMessage("Número de vagas deve ser maior que zero");
        RuleFor(c => c.ImagemUrl)
            .MaximumLength(500).WithMessage("URL da imagem deve ter no máximo 500 caracteres")
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("A URL da imagem do curso deve ser uma URL válida");
        RuleFor(c => c.ValidoAte)
            .GreaterThanOrEqualTo(DateTime.Now).When(c => c.ValidoAte.HasValue)
            .WithMessage("A Data de Validade não pode ser no passado");

        // Conteúdo Programático
        RuleFor(c => c.Resumo)
            .NotEmpty().WithMessage("Resumo é obrigatório");
        RuleFor(c => c.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória");
        RuleFor(c => c.Objetivos)
            .NotEmpty().WithMessage("Objetivos são obrigatórios");
    }
}
