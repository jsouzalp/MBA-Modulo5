using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.Settings;

[ExcludeFromCodeCoverage]
public class ResilienceSettings
{
    public int RetryCount { get; set; } = 3;
    public int CircuitBreakerThreshold { get; set; } = 3;
    public TimeSpan CircuitBreakerDuration { get; set; } = TimeSpan.FromSeconds(30);
    public TimeSpan TimeoutDuration { get; set; } = TimeSpan.FromSeconds(30);
}
