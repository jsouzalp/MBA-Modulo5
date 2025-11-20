using Core.Identidade;

namespace Core.Tests.Identidade;

public class AppSettingsTests 
{
    [Fact]
    public void AppSettings_DeveCriarComPropriedadesPadrao()
    {
        // Arrange & Act
        var appSettings = new AppSettings();

        // Assert
        appSettings.Should().NotBeNull();
        appSettings.AutenticacaoJwksUrl.Should().Be(string.Empty);
    }

    [Fact]
    public void AppSettings_DevePermitirDefinirAutenticacaoJwksUrl()
    {
        // Arrange
        var appSettings = new AppSettings();
        var url = "https://auth.example.com/.well-known/jwks.json";

        // Act
        appSettings.AutenticacaoJwksUrl = url;

        // Assert
        appSettings.AutenticacaoJwksUrl.Should().Be(url);
    }

    [Fact]
    public void AppSettings_DevePermitirUrlVazia()
    {
        // Arrange
        var appSettings = new AppSettings();

        // Act
        appSettings.AutenticacaoJwksUrl = string.Empty;

        // Assert
        appSettings.AutenticacaoJwksUrl.Should().Be(string.Empty);
    }

    [Fact]
    public void AppSettings_DevePermitirUrlNull()
    {
        // Arrange
        var appSettings = new AppSettings();

        // Act
        appSettings.AutenticacaoJwksUrl = null!;

        // Assert
        appSettings.AutenticacaoJwksUrl.Should().BeNull();
    }

    [Fact]
    public void AppSettings_DevePermitirUrlHttps()
    {
        // Arrange
        var appSettings = new AppSettings();
        var url = "https://secure.example.com/jwks";

        // Act
        appSettings.AutenticacaoJwksUrl = url;

        // Assert
        appSettings.AutenticacaoJwksUrl.Should().Be(url);
    }

    [Fact]
    public void AppSettings_DevePermitirUrlHttp()
    {
        // Arrange
        var appSettings = new AppSettings();
        var url = "http://example.com/jwks";

        // Act
        appSettings.AutenticacaoJwksUrl = url;

        // Assert
        appSettings.AutenticacaoJwksUrl.Should().Be(url);
    }

    [Fact]
    public void AppSettings_DevePermitirUrlLocalhost()
    {
        // Arrange
        var appSettings = new AppSettings();
        var url = "https://localhost:5001/.well-known/jwks.json";

        // Act
        appSettings.AutenticacaoJwksUrl = url;

        // Assert
        appSettings.AutenticacaoJwksUrl.Should().Be(url);
    }

    [Fact]
    public void AppSettings_DeveManterPropriedadesIndependentes()
    {
        // Arrange
        var appSettings1 = new AppSettings();
        var appSettings2 = new AppSettings();

        // Act
        appSettings1.AutenticacaoJwksUrl = "https://auth1.example.com/jwks";
        appSettings2.AutenticacaoJwksUrl = "https://auth2.example.com/jwks";

        // Assert
        appSettings1.AutenticacaoJwksUrl.Should().Be("https://auth1.example.com/jwks");
        appSettings2.AutenticacaoJwksUrl.Should().Be("https://auth2.example.com/jwks");
    }

    [Fact]
    public void AppSettings_DevePermitirAlteracaoDePropriedades()
    {
        // Arrange
        var appSettings = new AppSettings();
        var url1 = "https://auth1.example.com/jwks";
        var url2 = "https://auth2.example.com/jwks";

        // Act
        appSettings.AutenticacaoJwksUrl = url1;
        appSettings.AutenticacaoJwksUrl = url2;

        // Assert
        appSettings.AutenticacaoJwksUrl.Should().Be(url2);
    }
}
