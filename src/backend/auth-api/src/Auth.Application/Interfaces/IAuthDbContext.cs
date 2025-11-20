using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Interfaces;

public interface IAuthDbContext
{
    DbSet<ApplicationUser> Users { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
