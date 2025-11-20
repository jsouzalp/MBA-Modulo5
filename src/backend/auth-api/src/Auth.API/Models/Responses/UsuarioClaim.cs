using System.Diagnostics.CodeAnalysis;

namespace Auth.API.Models.Responses;

[ExcludeFromCodeCoverage]
public class UsuarioClaim
{
    public string Value { get; set; }
    public string Type { get; set; }
}
