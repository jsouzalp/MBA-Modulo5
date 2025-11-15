using Auth.Application.DTOs;

namespace Auth.UnitTests.Application.DTOs;

public class UsuarioRespostaLoginDtoTests : TestBase
{
    [Fact]
    public void UsuarioRespostaLoginDto_DeveCriarDtoValido()
    {
        // Arrange
        var accessToken = Faker.Random.AlphaNumeric(100);
        var refreshToken = Guid.NewGuid();
        var expiresIn = Faker.Random.Double(3600, 7200);
        var usuarioToken = new UsuarioTokenDto
        {
            Id = Faker.Random.Guid().ToString(),
            Email = Faker.Person.Email,
            Claims = new List<UsuarioClaimDto>()
        };

        // Act
        var dto = new UsuarioRespostaLoginDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = expiresIn,
            UsuarioToken = usuarioToken
        };

        // Assert
        dto.Should().NotBeNull();
        dto.AccessToken.Should().Be(accessToken);
        dto.RefreshToken.Should().Be(refreshToken);
        dto.ExpiresIn.Should().Be(expiresIn);
        dto.UsuarioToken.Should().Be(usuarioToken);
    }

    [Fact]
    public void UsuarioRespostaLoginDto_DevePermitirAlteracaoDePropriedades()
    {
        // Arrange
        var dto = new UsuarioRespostaLoginDto();
        var novoAccessToken = Faker.Random.AlphaNumeric(100);
        var novoRefreshToken = Guid.NewGuid();

        // Act
        dto.AccessToken = novoAccessToken;
        dto.RefreshToken = novoRefreshToken;

        // Assert
        dto.AccessToken.Should().Be(novoAccessToken);
        dto.RefreshToken.Should().Be(novoRefreshToken);
    }

    [Fact]
    public void UsuarioRespostaLoginDto_DeveInicializarComValoresPadrao()
    {
        // Act
        var dto = new UsuarioRespostaLoginDto();

        // Assert
        dto.AccessToken.Should().BeNull();
        dto.RefreshToken.Should().Be(Guid.Empty);
        dto.ExpiresIn.Should().Be(0);
        dto.UsuarioToken.Should().BeNull();
    }
}
