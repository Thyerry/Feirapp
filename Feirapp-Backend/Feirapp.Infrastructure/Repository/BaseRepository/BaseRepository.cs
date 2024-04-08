using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository.BaseRepository;

public class BaseRepository<T> : IBaseRepository<T>, IDisposable where T : class
{
    private readonly BaseContext _context;

    protected BaseRepository(BaseContext context)
    {
        var options = new DbContextOptions<BaseContext>();
        _context = context ?? new BaseContext(options);
    }

    public async Task<T> InsertAsync(T entity, CancellationToken ct)
    {
        var result = await _context.Set<T>().AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return result.Entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken ct)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        _context.Set<T>().Remove(await GetByIdAsync(id, ct));
        await _context.SaveChangesAsync(ct);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<T>().ToListAsync(cancellationToken: ct);
    }

    public async Task<T> GetByIdAsync(long id, CancellationToken ct)
    {
        return await _context.Set<T>().FindAsync(id) ?? throw new InvalidOperationException();
    }

    public void Dispose()
    {
    }
}