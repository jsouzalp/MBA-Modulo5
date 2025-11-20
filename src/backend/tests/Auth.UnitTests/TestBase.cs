using Bogus;

namespace Auth.UnitTests;

public abstract class TestBase
{
    protected readonly Faker Faker;

    protected TestBase()
    {
        Faker = new Faker("pt_BR");
    }

    protected static void AssertSuccess<T>(T result) where T : class
    {
        result.Should().NotBeNull();
    }

    protected static void AssertFailure<T>(T result) where T : class
    {
        result.Should().BeNull();
    }

    protected static Mock<T> CreateMock<T>() where T : class
    {
        return new Mock<T>();
    }
}
