using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Feirapp.Infrastructure.Extensions;

public static class DbSetExtensions
{
    public static async Task<EntityEntry<T>> AddIfNotExistsAsync<T>(this DbSet<T> dbSet, T entity, Func<T, bool>? predicate, CancellationToken ct = default) where T : class
    {
        var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
        return !exists ? await dbSet.AddAsync(entity, ct) : null;
    }
}