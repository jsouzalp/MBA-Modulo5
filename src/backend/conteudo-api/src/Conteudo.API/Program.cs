using Conteudo.API.Configuration;
using Conteudo.API.Helpers;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddApiConfiguration();

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerConfiguration();
        }

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", DateTime = DateTime.UtcNow }))
            .WithName("HealthCheck")
            .WithOpenApi();

        app.UseDbMigrationHelper();
        app.Run();
    }
}
