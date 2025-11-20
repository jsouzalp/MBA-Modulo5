using FluentValidation;

namespace Alunos.Application.Commands.CadastrarAluno;

public class CadastrarAlunoCommandValidator : AbstractValidator<CadastrarAlunoCommand>
{
    public CadastrarAlunoCommandValidator()
    {
        RuleFor(c => c.Id).NotEqual(Guid.Empty).WithMessage("Id do aluno inválido.");
        RuleFor(c => c.Nome).NotEmpty().WithMessage("Nome é obrigatório.")
            .Length(3, 100).WithMessage("Nome deve ter entre 3 e 100 caracteres");

        RuleFor(c => c.Email).NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.")
            .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres");

        RuleFor(c => c.Cpf).NotEmpty().WithMessage("CPF é obrigatório")
            .Length(11).WithMessage("CPF deve ter 11 caracteres");

        RuleFor(c => c.DataNascimento).LessThan(DateTime.Today).WithMessage("Data de nascimento deve ser no passado.");

        RuleFor(c => c.Telefone).MaximumLength(25).WithMessage("Telefone deve ter no máximo 25 caracteres");

        RuleFor(c => c.Genero).MaximumLength(20).WithMessage("Gênero deve ter no máximo 20 caracteres");

        RuleFor(c => c.Cidade).MaximumLength(50).WithMessage("Cidade deve ter no máximo 50 caracteres");

        RuleFor(c => c.Estado).MaximumLength(2).WithMessage("Estado deve ter no máximo 2 caracteres");

        RuleFor(c => c.Cep).MaximumLength(8).WithMessage("CEP deve ter no máximo 8 caracteres");

        RuleFor(c => c.Foto).MaximumLength(1024).WithMessage("URL da foto deve ter no máximo 1024 caracteres");
    }
}
