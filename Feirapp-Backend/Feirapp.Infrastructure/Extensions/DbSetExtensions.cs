using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Extensions;

public static class DbSetExtensions
{
    public static async Task<T> AddIfNotExistsAsync<T>(this DbSet<T> dbSet, T entity, Func<T, bool>? predicate, CancellationToken ct = default) where T : class
    {
        var exists = dbSet.Where(predicate).FirstOrDefault();
        if (exists != null) return exists;
        
        var result = await dbSet.AddAsync(entity, ct);
        return result.Entity;
    }
}