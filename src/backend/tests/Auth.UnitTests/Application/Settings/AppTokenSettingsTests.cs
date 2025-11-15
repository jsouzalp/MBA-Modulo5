using Auth.Application.Settings;

namespace Auth.UnitTests.Application.Settings;

public class AppTokenSettingsTests : TestBase
{
    [Fact]
    public void AppTokenSettings_DeveInicializarComValorPadrao()
    {
        // Act
        var settings = new AppTokenSettings();

        // Assert
        settings.Should().NotBeNull();
        settings.RefreshTokenExpiration.Should().Be(0);
    }

    [Fact]
    public void AppTokenSettings_DevePermitirAlteracaoDePropriedades()
    {
        // Arrange
        var settings = new AppTokenSettings();
        var novoRefreshTokenExpiration = Faker.Random.Int(1, 365);

        // Act
        settings.RefreshTokenExpiration = novoRefreshTokenExpiration;

        // Assert
        settings.RefreshTokenExpiration.Should().Be(novoRefreshTokenExpiration);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(7)]
    [InlineData(30)]
    [InlineData(90)]
    [InlineData(365)]
    public void AppTokenSettings_RefreshTokenExpiration_DeveAceitarValoresValidos(int refreshTokenExpiration)
    {
        // Arrange
        var settings = new AppTokenSettings();

        // Act
        settings.RefreshTokenExpiration = refreshTokenExpiration;

        // Assert
        settings.RefreshTokenExpiration.Should().Be(refreshTokenExpiration);
    }

    [Fact]
    public void AppTokenSettings_DeveSerImutavelAposConfiguracao()
    {
        // Arrange
        var settings = new AppTokenSettings
        {
            RefreshTokenExpiration = 30
        };

        // Act & Assert
        settings.RefreshTokenExpiration.Should().Be(30);
    }

    [Fact]
    public void AppTokenSettings_DevePermitirValorZero()
    {
        // Arrange
        var settings = new AppTokenSettings();

        // Act
        settings.RefreshTokenExpiration = 0;

        // Assert
        settings.RefreshTokenExpiration.Should().Be(0);
    }

    [Fact]
    public void AppTokenSettings_DevePermitirValoresNegativos()
    {
        // Arrange
        var settings = new AppTokenSettings();
        var valorNegativo = Faker.Random.Int(-365, -1);

        // Act
        settings.RefreshTokenExpiration = valorNegativo;

        // Assert
        settings.RefreshTokenExpiration.Should().Be(valorNegativo);
    }
}
