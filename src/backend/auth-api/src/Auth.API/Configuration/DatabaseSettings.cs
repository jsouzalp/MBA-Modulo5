using System.Diagnostics.CodeAnalysis;

namespace Auth.API.Configuration;

[ExcludeFromCodeCoverage]
public class DatabaseSettings
{
    public string DefaultConnection { get; set; } = string.Empty;
    public string DevelopmentConnection { get; set; } = "Data Source=../../../../../data/auth-dev.db";
    public string ProductionConnection { get; set; } = "Server=localhost;Database=AuthDB;Trusted_Connection=true;TrustServerCertificate=true;";
}
