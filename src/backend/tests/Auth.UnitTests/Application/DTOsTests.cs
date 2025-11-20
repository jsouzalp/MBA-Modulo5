using Auth.Application.DTOs;

namespace Auth.UnitTests.Application;

public class DTOsTests : TestBase
{
    [Fact]
    public void UsuarioRespostaLoginDto_DeveSerCriadoComSucesso()
    {
        // Arrange
        var accessToken = Faker.Random.AlphaNumeric(100);
        var refreshToken = Guid.NewGuid();
        var expiresIn = Faker.Random.Double(3600, 7200);
        var usuarioToken = new UsuarioTokenDto
        {
            Id = Guid.NewGuid().ToString(),
            Email = Faker.Person.Email,
            Claims = new List<UsuarioClaimDto>
            {
                new UsuarioClaimDto { Type = "role", Value = "Usuario" }
            }
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
    public void UsuarioTokenDto_DeveSerCriadoComSucesso()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var email = Faker.Person.Email;
        var claims = new List<UsuarioClaimDto>
        {
            new UsuarioClaimDto { Type = "role", Value = "Usuario" },
            new UsuarioClaimDto { Type = "sub", Value = id }
        };

        // Act
        var dto = new UsuarioTokenDto
        {
            Id = id,
            Email = email,
            Claims = claims
        };

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(id);
        dto.Email.Should().Be(email);
        dto.Claims.Should().HaveCount(2);
        dto.Claims.Should().Contain(c => c.Type == "role" && c.Value == "Usuario");
        dto.Claims.Should().Contain(c => c.Type == "sub" && c.Value == id);
    }

    [Fact]
    public void UsuarioClaimDto_DeveSerCriadoComSucesso()
    {
        // Arrange
        var type = "role";
        var value = "Administrador";

        // Act
        var dto = new UsuarioClaimDto
        {
            Type = type,
            Value = value
        };

        // Assert
        dto.Should().NotBeNull();
        dto.Type.Should().Be(type);
        dto.Value.Should().Be(value);
    }

    [Fact]
    public void UsuarioRespostaLoginDto_DevePermitirAlteracaoDePropriedades()
    {
        // Arrange
        var dto = new UsuarioRespostaLoginDto();

        // Act
        dto.AccessToken = "novo-token";
        dto.RefreshToken = Guid.NewGuid();
        dto.ExpiresIn = 7200;
        dto.UsuarioToken = new UsuarioTokenDto();

        // Assert
        dto.AccessToken.Should().Be("novo-token");
        dto.RefreshToken.Should().NotBeEmpty();
        dto.ExpiresIn.Should().Be(7200);
        dto.UsuarioToken.Should().NotBeNull();
    }

    [Fact]
    public void UsuarioTokenDto_DevePermitirAlteracaoDePropriedades()
    {
        // Arrange
        var dto = new UsuarioTokenDto();

        // Act
        dto.Id = "novo-id";
        dto.Email = "novo@email.com";
        dto.Claims = new List<UsuarioClaimDto>();

        // Assert
        dto.Id.Should().Be("novo-id");
        dto.Email.Should().Be("novo@email.com");
        dto.Claims.Should().NotBeNull();
        dto.Claims.Should().BeEmpty();
    }

    [Fact]
    public void UsuarioClaimDto_DevePermitirAlteracaoDePropriedades()
    {
        // Arrange
        var dto = new UsuarioClaimDto();

        // Act
        dto.Type = "novo-tipo";
        dto.Value = "novo-valor";

        // Assert
        dto.Type.Should().Be("novo-tipo");
        dto.Value.Should().Be("novo-valor");
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("abc")]
    [InlineData("abcd")]
    [InlineData("abcde")]
    [InlineData("abcdef")]
    [InlineData("abcdefg")]
    [InlineData("abcdefgh")]
    [InlineData("abcdefghi")]
    [InlineData("abcdefghij")]
    public void UsuarioRespostaLoginDto_DeveAceitarTokensDeDiferentesTamanhos(string token)
    {
        // Arrange & Act
        var dto = new UsuarioRespostaLoginDto
        {
            AccessToken = token
        };

        // Assert
        dto.AccessToken.Should().Be(token);
    }

    [Fact]
    public void UsuarioRespostaLoginDto_DeveAceitarExpiresInZero()
    {
        // Arrange & Act
        var dto = new UsuarioRespostaLoginDto
        {
            ExpiresIn = 0
        };

        // Assert
        dto.ExpiresIn.Should().Be(0);
    }

    [Fact]
    public void UsuarioRespostaLoginDto_DeveAceitarExpiresInNegativo()
    {
        // Arrange & Act
        var dto = new UsuarioRespostaLoginDto
        {
            ExpiresIn = -1
        };

        // Assert
        dto.ExpiresIn.Should().Be(-1);
    }
}
