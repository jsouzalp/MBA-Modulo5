namespace Auth.Application.Settings;

public class JwtSettings
{
    public int ExpiryMinutes { get; set; } = 60;
    public int RefreshTokenExpirationDays { get; set; } = 7;
}
