using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Extensions;

public static class DbSetExtensions
{
    public static async Task AddIfNotExistsAsync<T>(this DbSet<T> dbSet, T entity, Func<T, bool>? predicate, CancellationToken ct = default) where T : class
    {
        var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
        if (exists) return;
        
        await dbSet.AddAsync(entity, ct);
    }
}