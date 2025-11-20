using BFF.API.Services.Aluno;
using BFF.API.Services.Conteudos;
using BFF.API.Services.Pagamentos;
using BFF.Application.Interfaces.Services;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using Core.Utils;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Configuration;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<INotificador, Notificador>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<INotificationHandler<DomainNotificacaoRaiz>, DomainNotificacaoHandler>();

        services.AddScoped<IConteudoService, ConteudoService>();
        services.AddScoped<IAlunoService, AlunoService>();
        services.AddScoped<IPagamentoService, PagamentoService>();

        services.AddScoped<ICacheService, Infrastructure.Services.CacheService>();
        services.AddTransient<IApiClientService, Infrastructure.Services.ApiClientService>();
        services.AddScoped<IDashboardService, Infrastructure.Services.DashboardService>();

        services.RegisterNotification();
    }
}
