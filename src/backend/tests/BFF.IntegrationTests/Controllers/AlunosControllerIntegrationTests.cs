using BFF.API.Controllers;
using BFF.API.Services.Aluno;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BFF.IntegrationTests.Controllers;

public class AlunosControllerIntegrationTests
{
    [Fact]
    public void AlunosController_DeveTerTipoCorreto()
    {
        // Arrange & Act
        var controllerType = typeof(AlunosController);

        // Assert
        controllerType.Should().NotBeNull();
        controllerType.Should().BeAssignableTo<ControllerBase>();
    }

    [Fact]
    public void AlunosController_DeveHerdarDeBffController()
    {
        // Arrange & Act
        var controllerType = typeof(AlunosController);

        // Assert
        controllerType.BaseType.Should().Be(typeof(BffController));
    }

    [Fact]
    public void AlunosController_DeveTerConstrutorCorreto()
    {
        // Arrange & Act
        var controllerType = typeof(AlunosController);
        var constructors = controllerType.GetConstructors();

        // Assert
        constructors.Should().HaveCount(1);
        var constructor = constructors.First();
        var parameters = constructor.GetParameters();
        parameters.Should().HaveCount(4);
        parameters[0].ParameterType.Should().Be(typeof(IAlunoService));
        parameters[1].ParameterType.Should().Be(typeof(IMediatorHandler));
        parameters[2].ParameterType.Should().Be(typeof(INotificationHandler<DomainNotificacaoRaiz>));
        parameters[3].ParameterType.Should().Be(typeof(INotificador));
    }

    [Fact]
    public void AlunosController_DeveTerMetodosPublicos()
    {
        // Arrange & Act
        var controllerType = typeof(AlunosController);
        var publicMethods = controllerType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        // Assert
        publicMethods.Should().NotBeEmpty();
        publicMethods.Should().Contain(m => m.Name == "ObterAlunoPorIdAsync");
        publicMethods.Should().Contain(m => m.Name == "ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync");
    }

    [Fact]
    public void AlunosController_DeveTerAtributosCorretos()
    {
        // Arrange & Act
        var controllerType = typeof(AlunosController);

        // Assert
        controllerType.GetCustomAttributes(typeof(ApiControllerAttribute), false).Should().NotBeEmpty();
        controllerType.GetCustomAttributes(typeof(RouteAttribute), false).Should().NotBeEmpty();
    }
}
