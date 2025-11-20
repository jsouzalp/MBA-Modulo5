using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;
}
