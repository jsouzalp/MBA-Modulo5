using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.Settings;

[ExcludeFromCodeCoverage]
public class CacheSettings
{
    public TimeSpan DashboardExpiration { get; set; } = TimeSpan.FromMinutes(10);
    public TimeSpan UserProfileExpiration { get; set; } = TimeSpan.FromMinutes(60);
    public TimeSpan CursoDetailsExpiration { get; set; } = TimeSpan.FromMinutes(15);
    public TimeSpan RelatoriosExpiration { get; set; } = TimeSpan.FromMinutes(5);
}
