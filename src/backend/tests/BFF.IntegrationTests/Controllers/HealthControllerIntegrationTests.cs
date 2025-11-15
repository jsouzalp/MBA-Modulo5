using BFF.API.Controllers;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BFF.IntegrationTests.Controllers;

public class HealthControllerIntegrationTests
{
    [Fact]
    public void HealthController_DeveTerTipoCorreto()
    {
        // Arrange & Act
        var controllerType = typeof(HealthController);

        // Assert
        controllerType.Should().NotBeNull();
        controllerType.Should().BeAssignableTo<ControllerBase>();
    }

    [Fact]
    public void HealthController_DeveHerdarDeBffController()
    {
        // Arrange & Act
        var controllerType = typeof(HealthController);

        // Assert
        controllerType.BaseType.Should().Be(typeof(BffController));
    }

    [Fact]
    public void HealthController_DeveTerConstrutorCorreto()
    {
        // Arrange & Act
        var controllerType = typeof(HealthController);
        var constructors = controllerType.GetConstructors();

        // Assert
        constructors.Should().HaveCount(1);
        var constructor = constructors.First();
        var parameters = constructor.GetParameters();
        parameters.Should().HaveCount(3);
        parameters[0].ParameterType.Should().Be(typeof(IMediatorHandler));
        parameters[1].ParameterType.Should().Be(typeof(INotificationHandler<DomainNotificacaoRaiz>));
        parameters[2].ParameterType.Should().Be(typeof(INotificador));
    }

    [Fact]
    public void HealthController_DeveTerMetodosPublicos()
    {
        // Arrange & Act
        var controllerType = typeof(HealthController);
        var publicMethods = controllerType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        // Assert
        publicMethods.Should().NotBeEmpty();
        publicMethods.Should().Contain(m => m.Name == "Obter");
        publicMethods.Should().Contain(m => m.Name == "ObterStatus");
    }

    [Fact]
    public void HealthController_DeveTerAtributosCorretos()
    {
        // Arrange & Act
        var controllerType = typeof(HealthController);

        // Assert
        controllerType.GetCustomAttributes(typeof(ApiControllerAttribute), false).Should().NotBeEmpty();
        controllerType.GetCustomAttributes(typeof(RouteAttribute), false).Should().NotBeEmpty();
    }
}
