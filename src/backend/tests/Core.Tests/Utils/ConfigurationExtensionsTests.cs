using Core.Utils;
using Microsoft.Extensions.Configuration;

namespace Core.Tests.Utils;

public class ConfigurationExtensionsTests
{
    [Fact]
    public void GetMessageQueueConnection_DeveRetornarValorQuandoConfigurado()
    {
        // Arrange
        var configuration = CreateConfigurationWithMessageQueue("test-connection");

        // Act
        var result = configuration.GetMessageQueueConnection("test-connection");

        // Assert
        result.Should().Be("test-connection");
    }

    [Fact]
    public void GetMessageQueueConnection_DeveRetornarNullQuandoNaoConfigurado()
    {
        // Arrange
        var configuration = CreateEmptyConfiguration();

        // Act
        var result = configuration.GetMessageQueueConnection("inexistente");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetMessageQueueConnection_DeveRetornarNullQuandoConfigurationNull()
    {
        // Arrange
        IConfiguration? configuration = null;

        // Act
        var result = configuration.GetMessageQueueConnection("teste");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetMessageQueueConnection_DeveRetornarValorQuandoSectionExiste()
    {
        // Arrange
        var configuration = CreateConfigurationWithMessageQueue("test-connection");

        // Act
        var result = configuration.GetMessageQueueConnection("test-connection");

        // Assert
        result.Should().Be("test-connection");
    }

    [Fact]
    public void GetMessageQueueConnection_DeveRetornarNullQuandoSectionVazia()
    {
        // Arrange
        var configuration = CreateConfigurationWithEmptyMessageQueue();

        // Act
        var result = configuration.GetMessageQueueConnection("teste");

        // Assert
        result.Should().Be(string.Empty);
    }

    private static IConfiguration CreateConfigurationWithMessageQueue(string connectionString)
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"MessageQueueConnection:test-connection", connectionString}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    private static IConfiguration CreateConfigurationWithEmptyMessageQueue()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"MessageQueueConnection:teste", ""}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    private static IConfiguration CreateEmptyConfiguration()
    {
        return new ConfigurationBuilder().Build();
    }
}
