using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Conteudo.IntegrationTests;

public abstract class TestBase
{
    protected static void AssertSuccess<T>(T result)
    {
        result.Should().NotBeNull();
    }

    protected static void AssertFailure<T>(T result)
    {
        result.Should().BeNull();
    }
}

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.FirstOrDefault(d => d.ServiceType.Name.Contains("DbContext"));
            if (dbContextDescriptor != null)
                services.Remove(dbContextDescriptor);

            var dbMigrationDescriptor = services.FirstOrDefault(d => d.ServiceType.Name.Contains("DbMigration"));
            if (dbMigrationDescriptor != null)
                services.Remove(dbMigrationDescriptor);

            services.RemoveAll<JwtBearerHandler>();

            services.AddAuthentication("Test")
                .AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("Test", options => { });

            services.Configure<AuthenticationOptions>(options =>
            {
                options.DefaultScheme = "Test";
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            });
        });
    }
}

public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
{
    public TestAuthenticationHandler(
        IOptionsMonitor<TestAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "test@test.com"),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "Administrador"),
            new Claim(ClaimTypes.Role, "Usuario")
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

public class TestAuthenticationOptions : AuthenticationSchemeOptions
{
}
