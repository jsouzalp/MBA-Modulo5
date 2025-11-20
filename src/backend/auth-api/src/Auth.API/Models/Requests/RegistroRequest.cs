using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Auth.API.Models.Requests;

[ExcludeFromCodeCoverage]
public class RegistroRequest : IValidatableObject
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime DataNascimento { get; set; }

    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(14, MinimumLength = 11, ErrorMessage = "CPF deve ter entre 11 e 14 caracteres")]
    public string CPF { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
    public string Telefone { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Gênero deve ter no máximo 20 caracteres")]
    public string Genero { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Cidade deve ter no máximo 100 caracteres")]
    public string Cidade { get; set; } = string.Empty;

    [StringLength(2, ErrorMessage = "Estado deve ter no máximo 2 caracteres")]
    [Required(ErrorMessage = "Estado deve ter no máximo 2 caracteres")]
    public string Estado { get; set; } = string.Empty;

    [StringLength(10, ErrorMessage = "CEP deve ter no máximo 10 caracteres")]
    public string CEP { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "URL da foto deve ter no máximo 500 caracteres")]
    public string? Foto { get; set; }

    public bool EhAdministrador { get; set; } = false;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var idade = CalcularIdade(DataNascimento);
        if (idade < 16 || idade > 100)
        {
            yield return new ValidationResult("Idade deve estar entre 16 e 100 anos.", new[] { nameof(DataNascimento) });
        }
    }

    private static int CalcularIdade(DateTime nascimento)
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - nascimento.Year;
        if (nascimento.Date > hoje.AddYears(-idade)) idade--;
        return idade;
    }
}
