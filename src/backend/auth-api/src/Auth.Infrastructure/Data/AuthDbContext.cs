using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Auth.Infrastructure.Data;

[ExcludeFromCodeCoverage]
public class AuthDbContext : IdentityDbContext<ApplicationUser>, IAuthDbContext, ISecurityKeyContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<KeyMaterial> SecurityKeys { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DataNascimento).IsRequired();
            entity.Property(e => e.DataCadastro).IsRequired();
            entity.Property(e => e.Ativo).IsRequired();
        });
    }
}
