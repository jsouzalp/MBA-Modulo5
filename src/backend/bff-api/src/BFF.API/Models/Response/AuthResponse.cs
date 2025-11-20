using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Models.Response;

[ExcludeFromCodeCoverage]
public class AuthResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public string Endpoint { get; set; } = string.Empty;

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public UserInfo? User { get; set; }
}
