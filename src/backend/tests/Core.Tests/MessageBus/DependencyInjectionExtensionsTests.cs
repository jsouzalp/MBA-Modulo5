using global::MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Tests.MessageBus;

public class DependencyInjectionExtensionsTests 
{
    [Fact]
    public void AddMessageBus_DeveAdicionarMessageBusAoServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var connection = "host=localhost;port=5672";

        // Act
        var result = services.AddMessageBus(connection);

        // Assert
        result.Should().BeSameAs(services);
        var serviceProvider = services.BuildServiceProvider();
        var messageBus = serviceProvider.GetService<IMessageBus>();
        messageBus.Should().NotBeNull();
        messageBus.Should().BeOfType<global::MessageBus.MessageBus>();
    }

    [Fact]
    public void AddMessageBus_DeveRegistrarMessageBusComoSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        var connection = "host=localhost;port=5672";

        // Act
        services.AddMessageBus(connection);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var messageBus1 = serviceProvider.GetService<IMessageBus>();
        var messageBus2 = serviceProvider.GetService<IMessageBus>();

        messageBus1.Should().BeSameAs(messageBus2);
    }

    [Fact]
    public void AddMessageBus_DeveLancarArgumentNullExceptionQuandoConnectionNull()
    {
        // Arrange
        var services = new ServiceCollection();
        string? connection = null;

        // Act & Assert
        var action = () => services.AddMessageBus(connection!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddMessageBus_DeveLancarArgumentNullExceptionQuandoConnectionVazio()
    {
        // Arrange
        var services = new ServiceCollection();
        var connection = string.Empty;

        // Act & Assert
        var action = () => services.AddMessageBus(connection);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddMessageBus_DeveLancarArgumentNullExceptionQuandoConnectionEspacosEmBranco()
    {
        // Arrange
        var services = new ServiceCollection();
        var connection = "   ";

        // Act & Assert
        var action = () => services.AddMessageBus(connection);
        action.Should().Throw<EasyNetQ.EasyNetQException>();
    }

    [Fact]
    public void AddMessageBus_DevePermitirConnectionStringValida()
    {
        // Arrange
        var services = new ServiceCollection();
        var connection = "host=localhost;port=5672;username=guest;password=guest";

        // Act
        var result = services.AddMessageBus(connection);

        // Assert
        result.Should().BeSameAs(services);
        var serviceProvider = services.BuildServiceProvider();
        var messageBus = serviceProvider.GetService<IMessageBus>();
        messageBus.Should().NotBeNull();
    }

    [Fact]
    public void AddMessageBus_DeveRetornarServicesParaChaining()
    {
        // Arrange
        var services = new ServiceCollection();
        var connection = "host=localhost;port=5672";

        // Act
        var result = services.AddMessageBus(connection);

        // Assert
        result.Should().BeSameAs(services);
        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddMessageBus_DeveFuncionarComServicesVazio()
    {
        // Arrange
        var services = new ServiceCollection();
        var connection = "host=localhost;port=5672";

        // Act
        var result = services.AddMessageBus(connection);

        // Assert
        result.Should().NotBeNull();
        services.Count.Should().Be(1);
    }

    [Fact]
    public void AddMessageBus_DeveFuncionarComServicesComOutrosServicos()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<object>(new object());
        var connection = "host=localhost;port=5672";

        // Act
        var result = services.AddMessageBus(connection);

        // Assert
        result.Should().NotBeNull();
        services.Count.Should().Be(2);
        var serviceProvider = services.BuildServiceProvider();
        var messageBus = serviceProvider.GetService<IMessageBus>();
        messageBus.Should().NotBeNull();
    }
}
