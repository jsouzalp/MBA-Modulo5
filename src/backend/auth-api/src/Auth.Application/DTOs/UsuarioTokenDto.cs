using System.Diagnostics.CodeAnalysis;

namespace Auth.Application.DTOs;

[ExcludeFromCodeCoverage]
public class UsuarioTokenDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<UsuarioClaimDto> Claims { get; set; }
}
