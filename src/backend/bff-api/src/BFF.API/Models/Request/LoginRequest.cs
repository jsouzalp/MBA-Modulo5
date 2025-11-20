using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Models.Request;

[ExcludeFromCodeCoverage]
public class LoginRequest
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    public string Senha { get; set; } = string.Empty;
}
