using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Application.Settings;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.Application.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IAuthDbContext context,
    IJwtService jwtService,
    IHttpContextAccessor accessor,
    IOptions<AppTokenSettings> appTokenSettingsSettings)
{
    public readonly UserManager<ApplicationUser> UserManager = userManager;
    public readonly SignInManager<ApplicationUser> SignInManager = signInManager;
    private readonly AppTokenSettings _appTokenSettingsSettings = appTokenSettingsSettings.Value;

    public async Task<UsuarioRespostaLoginDto> GerarJwt(string email)
    {
        var user = await UserManager.FindByEmailAsync(email);
        var claims = await UserManager.GetClaimsAsync(user!);

        var identityClaims = await ObterClaimsUsuario(claims, user!);
        var encodedToken = await CodificarTokenAsync(identityClaims);

        var refreshToken = await GerarRefreshToken(email);

        return ObterRespostaToken(encodedToken, user!, claims, refreshToken);
    }

    public async Task<RefreshToken?> ObterRefreshToken(Guid refreshToken)
    {
        var token = await context.RefreshTokens.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Token == refreshToken);

        return token != null && token.ExpirationDate.ToLocalTime() > DateTime.Now ? token : null;
    }

    private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, ApplicationUser user)
    {
        var userRoles = await UserManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim("role", userRole));
        }

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private async Task<string> CodificarTokenAsync(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var currentIssuer =
            $"{accessor.HttpContext!.Request.Scheme}://{accessor.HttpContext!.Request.Host}";
        var key = await jwtService.GetCurrentSigningCredentials();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = currentIssuer,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = key
        });

        return tokenHandler.WriteToken(token);
    }

    private UsuarioRespostaLoginDto ObterRespostaToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims, RefreshToken refreshToken)
    {
        return new UsuarioRespostaLoginDto
        {
            AccessToken = encodedToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = TimeSpan.FromHours(1).TotalSeconds,
            UsuarioToken = new UsuarioTokenDto
            {
                Id = user.Id,
                Email = user.Email!,
                Claims = claims.Select(c => new UsuarioClaimDto { Type = c.Type, Value = c.Value })
            }
        };
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
            .TotalSeconds);

    private async Task<RefreshToken> GerarRefreshToken(string email)
    {
        var refreshToken = new RefreshToken
        {
            Username = email,
            ExpirationDate = DateTime.UtcNow.AddHours(_appTokenSettingsSettings.RefreshTokenExpiration)
        };

        context.RefreshTokens.RemoveRange(context.RefreshTokens.Where(u => u.Username == email));
        await context.RefreshTokens.AddAsync(refreshToken);

        await context.SaveChangesAsync();

        return refreshToken;
    }
}
