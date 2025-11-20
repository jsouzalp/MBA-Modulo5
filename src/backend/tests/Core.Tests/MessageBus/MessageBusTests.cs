using Core.Messages.Integration;
using global::MessageBus;

namespace Core.Tests.MessageBus;

public class MessageBusTests 
{
    private class TestIntegrationEvent : IntegrationEvent
    {
        public string Message { get; set; } = string.Empty;
    }

    private class TestResponseMessage : Core.Messages.ResponseMessage
    {
        public TestResponseMessage() : base(new FluentValidation.Results.ValidationResult())
        {
        }
    }

    [Fact]
    public void MessageBus_DeveImplementarIMessageBus()
    {
        // Arrange & Act
        var messageBusType = typeof(global::MessageBus.MessageBus);

        // Assert
        messageBusType.Should().Implement<IMessageBus>();
    }

    [Fact]
    public void MessageBus_DeveImplementarIDisposable()
    {
        // Arrange & Act
        var messageBusType = typeof(global::MessageBus.MessageBus);

        // Assert
        messageBusType.Should().Implement<IDisposable>();
    }

    [Fact]
    public void IMessageBus_DeveTerPropriedadesCorretas()
    {
        // Arrange & Act
        var interfaceType = typeof(IMessageBus);

        // Assert
        interfaceType.Should().NotBeNull();
        interfaceType.GetProperty("IsConnected").Should().NotBeNull();
        interfaceType.GetProperty("AdvancedBus").Should().NotBeNull();
    }

    [Fact]
    public void IMessageBus_DeveTerMetodosCorretos()
    {
        // Arrange & Act
        var interfaceType = typeof(IMessageBus);

        // Assert
        interfaceType.GetMethod("Publish").Should().NotBeNull();
        interfaceType.GetMethod("PublishAsync").Should().NotBeNull();
        interfaceType.GetMethod("Subscribe").Should().NotBeNull();
        interfaceType.GetMethod("SubscribeAsync").Should().NotBeNull();
        interfaceType.GetMethod("Request").Should().NotBeNull();
        interfaceType.GetMethod("RequestAsync").Should().NotBeNull();
        interfaceType.GetMethod("Respond").Should().NotBeNull();
        interfaceType.GetMethod("RespondAsync").Should().NotBeNull();
    }

    [Fact]
    public void IntegrationEvent_DeveHerdarDeEventRaiz()
    {
        // Arrange & Act
        var evento = new TestIntegrationEvent();

        // Assert
        evento.Should().NotBeNull();
        evento.Should().BeAssignableTo<Core.Messages.EventRaiz>();
    }

    [Fact]
    public void ResponseMessage_DeveTerValidationResult()
    {
        // Arrange & Act
        var response = new TestResponseMessage();

        // Assert
        response.Should().NotBeNull();
        response.ValidationResult.Should().NotBeNull();
    }

    [Fact]
    public void MessageBus_DeveTerEstruturaCorreta()
    {
        // Arrange & Act
        var messageBusType = typeof(global::MessageBus.MessageBus);

        // Assert
        messageBusType.Should().NotBeNull();
        messageBusType.GetConstructor(new[] { typeof(string) }).Should().NotBeNull();
    }
}
