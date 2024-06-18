using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository;

public class StoreRepository : BaseRepository<Store>, IStoreRepository, IDisposable
{
    private readonly BaseContext _context;

    public StoreRepository(BaseContext context) : base(context)
    {
        var options = new DbContextOptions<BaseContext>();
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<Store?> GetByCnpjAsync(string? storeCnpj, CancellationToken ct)
    {
        return _context.Stores
            .FirstOrDefaultAsync(x => x.Cnpj == storeCnpj, ct) ;
    }
}