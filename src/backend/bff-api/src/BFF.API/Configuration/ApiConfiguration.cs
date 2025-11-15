using BFF.API.Extensions;
using BFF.API.Filters;
using BFF.API.Handlers;
using BFF.API.Settings;
using BFF.Domain.Settings;
using Core.Identidade;
using Mapster;
using Polly;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace BFF.API.Configuration;

[ExcludeFromCodeCoverage]
public static class ApiConfiguration
{
    public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder)
    {
        return builder
            .AddConfiguration()
            .AddRedisConfiguration()
            .AddControllersConfiguration()
            .AddJwtConfiguration()
            .AddHttpContextAccessorConfiguration()
            .AddCorsConfiguration()
            .AddServicesConfiguration()
            .AddMapsterConfiguration()
            .AddSwaggerConfigurationExtension()
            .AddResilienceConfiguration();
    }

    private static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("JwtSettings"));
        builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
        builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
        builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
        builder.Services.Configure<ResilienceSettings>(builder.Configuration.GetSection("ResilienceSettings"));

        return builder;
    }

    private static WebApplicationBuilder AddRedisConfiguration(this WebApplicationBuilder builder)
    {
        // Redis Cache
        var redisSettings = builder.Configuration.GetSection("RedisSettings").Get<RedisSettings>();
        if (redisSettings != null && !string.IsNullOrEmpty(redisSettings.ConnectionString))
        {
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisSettings.ConnectionString;
                options.InstanceName = redisSettings.KeyPrefix;
            });
        }
        else
        {
            // Fallback para cache em memória se Redis não estiver disponível
            builder.Services.AddMemoryCache();
        }
        return builder;
    }

    private static WebApplicationBuilder AddControllersConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilter>();
        }).ConfigureApiBehaviorOptions(opt =>
        {
            opt.SuppressModelStateInvalidFilter = true;
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.WriteIndented = false;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

        return builder;
    }

    private static WebApplicationBuilder AddHttpContextAccessorConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        return builder;
    }

    private static WebApplicationBuilder AddCorsConfiguration(this WebApplicationBuilder builder)
    {
        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowedOrigins", policy =>
            {
                var corsSettings = builder.Configuration.GetSection("CORS");
                var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>();
                var allowedMethods = corsSettings.GetSection("AllowedMethods").Get<string[]>();
                var allowedHeaders = corsSettings.GetSection("AllowedHeaders").Get<string[]>();

                policy.WithOrigins(allowedOrigins ?? ["http://localhost:4200"])
                      .WithMethods(allowedMethods ?? ["GET", "POST", "PUT", "DELETE", "OPTIONS"])
                      .WithHeaders(allowedHeaders ?? ["Content-Type", "Authorization"])
                      .AllowCredentials();
            });
        });
        return builder;
    }

    private static WebApplicationBuilder AddServicesConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.RegisterServices();
        return builder;
    }

    private static WebApplicationBuilder AddMapsterConfiguration(this WebApplicationBuilder builder)
    {
        // Configurar Mapster
        TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);
        return builder;
    }

    private static WebApplicationBuilder AddJwtConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddJwtConfiguration(builder.Configuration);
        return builder;
    }

    private static WebApplicationBuilder AddSwaggerConfigurationExtension(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerConfiguration();
        return builder;
    }

    private static WebApplicationBuilder AddResilienceConfiguration(this WebApplicationBuilder builder)
    {
        // HttpClient para comunicação com outras APIs com Polly
        var resilienceSettings = builder.Configuration.GetSection("ResilienceSettings").Get<ResilienceSettings>();
        builder.Services.AddTransient<AuthorizationDelegatingHandler>();
        builder.Services.AddHttpClient("ApiClient")
            .AddHttpMessageHandler<AuthorizationDelegatingHandler>()
            .AddPolicyHandler(GetRetryPolicy(resilienceSettings))
            .AddPolicyHandler(GetCircuitBreakerPolicy(resilienceSettings));

        return builder;
    }

    // Métodos auxiliares para políticas de resiliência
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ResilienceSettings settings)
    {
        var retryCount = settings?.RetryCount ?? 3;

        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult(r =>
                r.StatusCode == HttpStatusCode.BadGateway ||
                r.StatusCode == HttpStatusCode.ServiceUnavailable ||
                r.StatusCode == HttpStatusCode.GatewayTimeout ||
                r.StatusCode == HttpStatusCode.RequestTimeout ||
                (int)r.StatusCode == 429)
            .WaitAndRetryAsync(
                retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Tentativa {retryCount} em {timespan}s");
                });
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ResilienceSettings settings)
    {
        var threshold = settings?.CircuitBreakerThreshold ?? 3;
        var duration = settings?.CircuitBreakerDuration ?? TimeSpan.FromSeconds(5);

        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult(r =>
                r.StatusCode == HttpStatusCode.BadGateway ||
                r.StatusCode == HttpStatusCode.ServiceUnavailable ||
                r.StatusCode == HttpStatusCode.GatewayTimeout ||
                r.StatusCode == HttpStatusCode.RequestTimeout ||
                (int)r.StatusCode == 429)
            .CircuitBreakerAsync(
                threshold,
                duration,
                onBreak: (outcome, duration) => Console.WriteLine($"Circuit breaker ABERTO por {duration}s"),
                onReset: () => Console.WriteLine("Circuit breaker FECHADO"));
    }
}
