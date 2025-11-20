using Conteudo.Application.Commands.AtualizarAula;
using Conteudo.Application.Commands.AtualizarCategoria;
using Conteudo.Application.Commands.AtualizarCurso;
using Conteudo.Application.Commands.AtualizarMaterial;
using Conteudo.Application.Commands.CadastrarAula;
using Conteudo.Application.Commands.CadastrarCategoria;
using Conteudo.Application.Commands.CadastrarCurso;
using Conteudo.Application.Commands.CadastrarMaterial;
using Conteudo.Application.Commands.ExcluirAula;
using Conteudo.Application.Commands.ExcluirCurso;
using Conteudo.Application.Commands.ExcluirMaterial;
using Conteudo.Application.Interfaces.Services;
using Conteudo.Application.Services;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Infrastructure.Data;
using Conteudo.Infrastructure.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Utils;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Conteudo.API.Configuration;

/// <summary>
/// Configura injeção de dependências para a API de Conteúdo
/// </summary>
[ExcludeFromCodeCoverage]
public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        // Application
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        // Commands
        services.AddScoped<INotificationHandler<DomainNotificacaoRaiz>, DomainNotificacaoHandler>();
        services.AddScoped<IRequestHandler<CadastrarCursoCommand, CommandResult>, CadastrarCursoCommandHandler>();
        services.AddScoped<IRequestHandler<CadastrarCategoriaCommand, CommandResult>, CadastrarCategoriaCommandHandler>();
        services.AddScoped<IRequestHandler<CadastrarAulaCommand, CommandResult>, CadastrarAulaCommandHandler>();
        services.AddScoped<IRequestHandler<CadastrarMaterialCommand, CommandResult>, CadastrarMaterialCommandHandler>();
        services.AddScoped<IRequestHandler<AtualizarCursoCommand, CommandResult>, AtualizarCursoCommandHandler>();
        services.AddScoped<IRequestHandler<AtualizarCategoriaCommand, CommandResult>, AtualizarCategoriaCommandHandler>();
        services.AddScoped<IRequestHandler<AtualizarAulaCommand, CommandResult>, AtualizarAulaCommandHandler>();
        services.AddScoped<IRequestHandler<AtualizarMaterialCommand, CommandResult>, AtualizarMaterialCommandHandler>();
        services.AddScoped<IRequestHandler<ExcluirCursoCommand, CommandResult>, ExcluirCursoCommandHandler>();
        services.AddScoped<IRequestHandler<ExcluirAulaCommand, CommandResult>, ExcluirAulaCommandHandler>();
        services.AddScoped<IRequestHandler<ExcluirMaterialCommand, CommandResult>, ExcluirMaterialCommandHandler>();

        // Services
        services.AddScoped<IAulaAppService, AulaAppService>();
        services.AddScoped<ICursoQuery, CursoQueryService>();
        services.AddScoped<ICategoriaAppService, CategoriaAppService>();
        services.AddScoped<IMaterialAppService, MaterialAppService>();

        // Data
        services.AddScoped<IAulaRepository, AulaRepository>();
        services.AddScoped<ICursoRepository, CursoRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<ConteudoDbContext>();

        // Notification
        services.RegisterNotification();
    }
}
