using System.Diagnostics.CodeAnalysis;

namespace Auth.Application.DTOs;

[ExcludeFromCodeCoverage]
public class UsuarioClaimDto
{
    public string Value { get; set; }
    public string Type { get; set; }
}
