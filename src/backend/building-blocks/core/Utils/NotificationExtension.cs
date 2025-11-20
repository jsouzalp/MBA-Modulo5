using Core.Notification;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Core.Utils;

[ExcludeFromCodeCoverage]
public static class NotificationExtension
{
    public static IServiceCollection RegisterNotification(this IServiceCollection services)
    {
        services.AddScoped<INotificador, Notificador>();
        return services;
    }
}
