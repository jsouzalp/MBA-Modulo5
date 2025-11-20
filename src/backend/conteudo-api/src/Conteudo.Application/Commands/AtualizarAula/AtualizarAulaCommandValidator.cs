using FluentValidation;

namespace Conteudo.Application.Commands.AtualizarAula
{
    public class AtualizarAulaCommandValidator : AbstractValidator<AtualizarAulaCommand>
    {
        public AtualizarAulaCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage("ID da aula é obrigatório");

            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty).WithMessage("ID do curso é obrigatório");

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");

            RuleFor(c => c.Descricao).NotEmpty()
                .WithMessage("Descrição é obrigatória");

            RuleFor(c => c.Numero)
                .GreaterThan(0).WithMessage("Número da aula deve ser maior que zero");

            RuleFor(c => c.DuracaoMinutos)
                .GreaterThan(0).WithMessage("Duração deve ser maior que zero");

            RuleFor(c => c.VideoUrl)
                .NotEmpty().WithMessage("URL do vídeo é obrigatória")
                .MaximumLength(500).WithMessage("URL do vídeo deve ter no máximo 500 caracteres");

            RuleFor(c => c.TipoAula)
                .NotEmpty().WithMessage("Tipo da aula é obrigatório")
                .MaximumLength(50).WithMessage("Tipo da aula deve ter no máximo 50 caracteres");
        }
    }
}
