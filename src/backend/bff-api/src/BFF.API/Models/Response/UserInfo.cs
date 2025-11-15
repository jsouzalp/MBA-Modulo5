using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Models.Response;

[ExcludeFromCodeCoverage]
public class UserInfo
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = new();
}
