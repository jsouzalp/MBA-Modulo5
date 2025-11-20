using Auth.Application.Settings;

namespace Auth.UnitTests.Application.Settings;

public class JwtSettingsTests : TestBase
{
    [Fact]
    public void JwtSettings_DeveInicializarComValoresPadrao()
    {
        // Act
        var settings = new JwtSettings();

        // Assert
        settings.Should().NotBeNull();
        settings.ExpiryMinutes.Should().Be(60);
        settings.RefreshTokenExpirationDays.Should().Be(7);
    }

    [Fact]
    public void JwtSettings_DevePermitirAlteracaoDePropriedades()
    {
        // Arrange
        var settings = new JwtSettings();
        var novoExpiryMinutes = Faker.Random.Int(30, 120);
        var novoRefreshTokenExpirationDays = Faker.Random.Int(1, 30);

        // Act
        settings.ExpiryMinutes = novoExpiryMinutes;
        settings.RefreshTokenExpirationDays = novoRefreshTokenExpirationDays;

        // Assert
        settings.ExpiryMinutes.Should().Be(novoExpiryMinutes);
        settings.RefreshTokenExpirationDays.Should().Be(novoRefreshTokenExpirationDays);
    }

    [Theory]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(120)]
    [InlineData(1440)]
    public void JwtSettings_ExpiryMinutes_DeveAceitarValoresValidos(int expiryMinutes)
    {
        // Arrange
        var settings = new JwtSettings();

        // Act
        settings.ExpiryMinutes = expiryMinutes;

        // Assert
        settings.ExpiryMinutes.Should().Be(expiryMinutes);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(7)]
    [InlineData(30)]
    [InlineData(90)]
    public void JwtSettings_RefreshTokenExpirationDays_DeveAceitarValoresValidos(int refreshTokenExpirationDays)
    {
        // Arrange
        var settings = new JwtSettings();

        // Act
        settings.RefreshTokenExpirationDays = refreshTokenExpirationDays;

        // Assert
        settings.RefreshTokenExpirationDays.Should().Be(refreshTokenExpirationDays);
    }

    [Fact]
    public void JwtSettings_DeveSerImutavelAposConfiguracao()
    {
        // Arrange
        var settings = new JwtSettings
        {
            ExpiryMinutes = 90,
            RefreshTokenExpirationDays = 14
        };

        // Act & Assert
        settings.ExpiryMinutes.Should().Be(90);
        settings.RefreshTokenExpirationDays.Should().Be(14);
    }
}
