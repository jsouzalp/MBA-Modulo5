using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Conteudo.Domain.Entities;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Conteudo.IntegrationTests.API.Controllers;

public class CategoriaControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly Mock<ICategoriaAppService> _categoriaAppServiceMock;
    private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
    private readonly MockDomainNotificacaoHandler _notificationsMock;
    private readonly Mock<INotificador> _notificadorMock;

    public CategoriaControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _categoriaAppServiceMock = new Mock<ICategoriaAppService>();
        _mediatorHandlerMock = new Mock<IMediatorHandler>();
        _notificationsMock = new MockDomainNotificacaoHandler();
        _notificadorMock = new Mock<INotificador>();
    }

    private Categoria CriarCategoria(string nome, string descricao, string cor)
    {
        return new Categoria(nome, descricao, cor, "", 0);
    }

    private HttpClient CreateClient()
    {
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ICategoriaAppService));
                if (descriptor != null)
                    services.Remove(descriptor);

                var mediatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IMediatorHandler));
                if (mediatorDescriptor != null)
                    services.Remove(mediatorDescriptor);

                var notificationsDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(INotificationHandler<DomainNotificacaoRaiz>));
                if (notificationsDescriptor != null)
                    services.Remove(notificationsDescriptor);

                var notificadorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(INotificador));
                if (notificadorDescriptor != null)
                    services.Remove(notificadorDescriptor);

                // Adicionar mocks
                services.AddScoped<ICategoriaAppService>(_ => _categoriaAppServiceMock.Object);
                services.AddScoped<IMediatorHandler>(_ => _mediatorHandlerMock.Object);
                services.AddScoped<INotificationHandler<DomainNotificacaoRaiz>>(_ => _notificationsMock);
                services.AddScoped<INotificador>(_ => _notificadorMock.Object);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task ObterPorId_ComIdValido_DeveRetornarCategoria()
    {
        // Arrange
        var client = CreateClient();
        var id = Guid.NewGuid();
        var categoriaDto = new CategoriaDto
        {
            Id = id,
            Nome = "Programação",
            Descricao = "Cursos de programação",
            Cor = "#FF0000"
        };

        _categoriaAppServiceMock.Setup(x => x.ObterPorIdAsync(id))
            .ReturnsAsync(categoriaDto);

        // Act
        var response = await client.GetAsync($"/api/Categoria/{id}");

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ObterPorId_ComIdInexistente_DeveRetornarNotFound()
    {
        // Arrange
        var client = CreateClient();
        var id = Guid.NewGuid();

        _categoriaAppServiceMock.Setup(x => x.ObterPorIdAsync(id))
            .ReturnsAsync((CategoriaDto?)null);

        _notificadorMock.Setup(x => x.AdicionarErro(It.IsAny<string>()));
        _notificadorMock.Setup(x => x.TemErros()).Returns(true);
        _notificadorMock.Setup(x => x.ObterErros()).Returns(new List<string> { "Categoria não encontrada." });

        // Act
        var response = await client.GetAsync($"/api/Categoria/{id}");

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ObterTodos_DeveRetornarListaDeCategorias()
    {
        // Arrange
        var client = CreateClient();
        var categorias = new List<CategoriaDto>
        {
            new CategoriaDto { Id = Guid.NewGuid(), Nome = "Programação", Descricao = "Cursos de programação", Cor = "#FF0000" },
            new CategoriaDto { Id = Guid.NewGuid(), Nome = "Design", Descricao = "Cursos de design", Cor = "#00FF00" }
        };

        _categoriaAppServiceMock.Setup(x => x.ObterTodasCategoriasAsync())
            .ReturnsAsync(categorias);

        // Act
        var response = await client.GetAsync("/api/Categoria");

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task CadastrarCategoria_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var client = CreateClient();
        var dto = new CadastroCategoriaDto
        {
            Nome = "Nova Categoria",
            Descricao = "Descrição da nova categoria",
            Cor = "#FF0000"
        };

        var validationResult = new ValidationResult();
        var commandResult = new CommandResult(validationResult, Guid.NewGuid());
        _mediatorHandlerMock.Setup(x => x.ExecutarComando(It.IsAny<RaizCommand>()))
            .ReturnsAsync(commandResult);

        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/Categoria", content);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task AtualizarCategoria_ComIdValido_DeveRetornarOk()
    {
        // Arrange
        var client = CreateClient();
        var id = Guid.NewGuid();
        var dto = new AtualizarCategoriaDto
        {
            Id = id,
            Nome = "Categoria Atualizada",
            Descricao = "Descrição atualizada",
            Cor = "#00FF00"
        };

        var validationResult = new ValidationResult();
        var commandResult = new CommandResult(validationResult, true);
        _mediatorHandlerMock.Setup(x => x.ExecutarComando(It.IsAny<RaizCommand>()))
            .ReturnsAsync(commandResult);

        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PutAsync($"/api/Categoria/{id}", content);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task AtualizarCategoria_ComIdDiferente_DeveRetornarBadRequest()
    {
        // Arrange
        var client = CreateClient();
        var id = Guid.NewGuid();
        var dto = new AtualizarCategoriaDto
        {
            Id = Guid.NewGuid(),
            Nome = "Categoria Atualizada",
            Descricao = "Descrição atualizada",
            Cor = "#00FF00"
        };

        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PutAsync($"/api/Categoria/{id}", content);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Contain("ID da categoria não confere");
    }
}

public class MockDomainNotificacaoHandler : DomainNotificacaoHandler
{
    private readonly List<DomainNotificacaoRaiz> _notificacoes = new();

    public new Task Handle(DomainNotificacaoRaiz notificacao, CancellationToken cancellationToken)
    {
        _notificacoes.Add(notificacao);
        return Task.CompletedTask;
    }

    public new List<string> ObterMensagens() => _notificacoes.Select(n => n.Valor).ToList();

    public new List<DomainNotificacaoRaiz> ObterNotificacoes() => _notificacoes;

    public new bool TemNotificacao() => _notificacoes.Count > 0;

    public new void Limpar() => _notificacoes.Clear();
}
