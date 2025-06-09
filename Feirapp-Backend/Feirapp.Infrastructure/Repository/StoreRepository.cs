using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository;

public class StoreRepository(BaseContext context) : IStoreRepository, IDisposable
{
    public Task<Store?> GetByCnpjAsync(string? storeCnpj, CancellationToken ct)
    {
        return context.Stores.FirstOrDefaultAsync(x => x.Cnpj == storeCnpj, ct);
    }

    public async Task<Store> AddIfNotExistsAsync(Func<Store, bool> query, Store store, CancellationToken ct)
    {
        return await context.Stores.AddIfNotExistsAsync(store, query, ct);
    }

    public async Task UpdateAsync(Store store, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<Store> InsertAsync(Store storeEntity, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Store>> GetAllAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<Store> GetByIdAsync(long storeId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}