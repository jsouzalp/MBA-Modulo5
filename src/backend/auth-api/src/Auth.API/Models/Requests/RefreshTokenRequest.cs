using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Auth.API.Models.Requests;

[ExcludeFromCodeCoverage]
public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Token de refresh é obrigatório")]
    public Guid RefreshToken { get; set; }
}
