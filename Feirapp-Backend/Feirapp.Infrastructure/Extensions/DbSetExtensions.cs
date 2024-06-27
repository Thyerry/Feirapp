using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Extensions;

public static class DbSetExtensions
{
    public static async Task<bool> AddIfNotExistsAsync<T>(this DbSet<T> dbSet, T entity, Func<T, bool>? predicate, CancellationToken ct = default) where T : class
    {
        var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
        if (exists) return false;
        
        await dbSet.AddAsync(entity, ct);
        return true;
    }
}