using Auth.Domain.Events;

namespace Auth.UnitTests.Domain.Events;
public class UserLoggedInEventTests
{
    [Fact]
    public void Ctor_deve_mapear_propriedades()
    {
        var id = Guid.NewGuid();
        var quando = new DateTime(2025, 1, 2, 3, 4, 5, DateTimeKind.Utc);

        var e = new UserLoggedInEvent(id, "a@b.com", "Fulano", quando, "127.0.0.1");

        e.UserId.Should().Be(id);
        e.Email.Should().Be("a@b.com");
        e.Nome.Should().Be("Fulano");
        e.DataLogin.Should().Be(quando);
        e.IpAddress.Should().Be("127.0.0.1");
    }
}
