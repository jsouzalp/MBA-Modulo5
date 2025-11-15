using System.Diagnostics.CodeAnalysis;

namespace Auth.API.Models.Responses;

[ExcludeFromCodeCoverage]
public class UsuarioRespostaLogin
{
    public string AccessToken { get; set; }
    public Guid RefreshToken { get; set; }
    public double ExpiresIn { get; set; }
    public UsuarioToken UsuarioToken { get; set; }
}
