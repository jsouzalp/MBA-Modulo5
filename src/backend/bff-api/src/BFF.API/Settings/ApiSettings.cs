using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Settings;

[ExcludeFromCodeCoverage]
public class ApiSettings
{
    public string AuthApiUrl { get; set; } = string.Empty;
    public string ConteudoApiUrl { get; set; } = string.Empty;
    public string AlunosApiUrl { get; set; } = string.Empty;
    public string PagamentosApiUrl { get; set; } = string.Empty;
}
