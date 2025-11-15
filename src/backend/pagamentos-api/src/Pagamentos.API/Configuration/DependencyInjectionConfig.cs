using Core.Mediator;
using Core.Messages;
using Core.Notification;
using MediatR;
using Microsoft.Extensions.Options;
using Pagamento.AntiCorruption.Interfaces;
using Pagamento.AntiCorruption.Services;
using Pagamentos.Application.Interfaces;
using Pagamentos.Application.Services;
using Pagamentos.Domain.Interfaces;
using Pagamentos.Domain.Services;
using Pagamentos.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace Pagamentos.API.Configuration;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionConfig
{
    public static WebApplicationBuilder AddDependencyInjectionConfig(this WebApplicationBuilder builder)
    {
        builder.Services.ResolveDependencies();
        return builder;
    }

    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        services.AddScoped<DomainNotificacaoHandler>();
        services.AddScoped<INotificationHandler<DomainNotificacaoRaiz>>(sp =>
                           sp.GetRequiredService<DomainNotificacaoHandler>());

        services.AddScoped<IMediatorHandler, MediatorHandler>();

        services.AddScoped<INotificador, Notificador>();

        services.AddScoped<IPagamentoConsultaAppService, PagamentoAppService>();
        services.AddScoped<IPagamentoComandoAppService, PagamentoAppService>();
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<IPagamentoService, PagamentoService>();
        services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
        services.AddScoped<IPayPalGateway, PayPalGateway>();
        services.AddScoped<Pagamento.AntiCorruption.Interfaces.IConfigurationManager, Pagamento.AntiCorruption.Services.ConfigurationManager>();

        return services;
    }
}
