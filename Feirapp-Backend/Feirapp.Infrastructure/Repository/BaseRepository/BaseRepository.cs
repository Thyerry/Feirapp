using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Feirapp.Infrastructure.Repository.BaseRepository;

public class BaseRepository<T> : IBaseRepository<T>, IDisposable where T : class
{
    private readonly BaseContext _context;

    public BaseRepository(BaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct)
    {
        return await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task<T> InsertAsync(T entity, CancellationToken ct)
    {
        var result = await _context.Set<T>().AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return result.Entity;
    }

    public async Task InsertListAsync(List<T> entities, CancellationToken ct)
    {
        await _context.Set<T>().BulkInsertAsync(entities, ct);
    }

    public async Task UpdateAsync(T entity, CancellationToken ct)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        _context.Set<T>().Remove(await GetByIdAsync(id, ct) ?? throw new InvalidOperationException("No entity found to delete"));
        await _context.SaveChangesAsync(ct);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<T>().ToListAsync(cancellationToken: ct);
    }

    public virtual async Task<T?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await _context.Set<T>().FindAsync(id, ct) ?? throw new InvalidOperationException();
    }

    public async Task<T> AddIfNotExistsAsync(Func<T, bool> predicate, T entity, CancellationToken ct = default)
    {
        var result = await _context.Set<T>().AddIfNotExistsAsync(entity, predicate, ct);
        await _context.SaveChangesAsync(ct);
        return result;
    }

    public List<T> GetByQuery(Func<T, bool> predicate, CancellationToken ct)
    {
        return _context.Set<T>().Where(predicate).ToList();
    }

    public void Dispose()
    {
    }
}