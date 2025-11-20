using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using BFF.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace BFF.Infrastructure.Services;

[ExcludeFromCodeCoverage]
public class DashboardService(
    ICacheService cacheService,
    IOptions<CacheSettings> cacheOptions,
    ILogger<DashboardService> logger) : IDashboardService
{
    private readonly CacheSettings _cacheSettings = cacheOptions.Value;

    public async Task<DashboardAdminDto> GetDashboardAdminAsync()
    {
        var cacheKey = "dashboard:admin";

        var cachedData = await cacheService.GetAsync<DashboardAdminDto>(cacheKey);
        if (cachedData != null)
        {
            logger.LogInformation("Dashboard do admin recuperado do cache");
            return cachedData;
        }

        logger.LogInformation("Carregando dashboard do admin dos microsserviços");

        await Task.Delay(100);

        var dashboardData = new DashboardAdminDto
        {
            EstatisticasAlunos = new EstatisticasAlunosDto
            {
                TotalAlunos = 350,
                AlunosAtivos = 280,
                AlunosInativos = 70,
                NovasMatriculasHoje = 12,
                NovasMatriculasSemana = 85,
                NovasMatriculasMes = 320,
                TaxaRetencao = 85.5m
            },
            EstatisticasCursos = new EstatisticasCursosDto
            {
                TotalCursos = 25,
                CursosAtivos = 20,
                CursosInativos = 5,
                MediaAvaliacoes = 4.5m,
                TotalAulas = 500,
                HorasConteudo = 1200
            },
            RelatorioVendas = new RelatorioVendasDto
            {
                VendasHoje = 1250.00m,
                VendasSemana = 8750.00m,
                VendasMes = 35000.00m,
                VendasAno = 420000.00m,
                TicketMedio = 125.50m,
                TotalTransacoes = 280,
                TaxaConversao = 12.5m
            },
            EstatisticasUsuarios = new EstatisticasUsuariosDto
            {
                TotalUsuarios = 375,
                UsuariosAtivos = 320,
                UsuariosOnline = 45,
                AdminsAtivos = 15,
                AlunosAtivos = 305
            },
            CursosPopulares = new List<CursoPopularDto>
            {
                new CursoPopularDto
                {
                    Id = Guid.NewGuid(),
                    Nome = "ASP.NET Core Completo",
                    TotalMatriculas = 150,
                    Receita = 29998.50m,
                    MediaAvaliacoes = 4.8m,
                    TotalAvaliacoes = 120
                },
                new CursoPopularDto
                {
                    Id = Guid.NewGuid(),
                    Nome = "C# Avançado",
                    TotalMatriculas = 95,
                    Receita = 18905.00m,
                    MediaAvaliacoes = 4.6m,
                    TotalAvaliacoes = 78
                }
            },
            VendasRecentes = new List<VendaRecenteDto>
            {
                new VendaRecenteDto
                {
                    Id = Guid.NewGuid(),
                    AlunoNome = "Maria Silva",
                    CursoNome = "ASP.NET Core Completo",
                    Valor = 199.99m,
                    DataVenda = DateTime.UtcNow.AddMinutes(-30),
                    Status = "Aprovado",
                    FormaPagamento = "Cartão de Crédito"
                },
                new VendaRecenteDto
                {
                    Id = Guid.NewGuid(),
                    AlunoNome = "João Santos",
                    CursoNome = "C# Avançado",
                    Valor = 149.99m,
                    DataVenda = DateTime.UtcNow.AddHours(-2),
                    Status = "Processando",
                    FormaPagamento = "PIX"
                }
            }
        };

        // Salva no cache usando a configuração específica para dashboard
        await cacheService.SetAsync(cacheKey, dashboardData, _cacheSettings.DashboardExpiration);

        logger.LogInformation("Dashboard do admin salvo no cache por {Minutes} minutos",
            _cacheSettings.DashboardExpiration.TotalMinutes);

        return dashboardData;
    }
}
