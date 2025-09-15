using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Domain.Services.Stores.Methods.SearchStores;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;
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

    public async Task<Store> InsertAsync(Store storeEntity, CancellationToken ct)
    {
        var existingStore = await context.Stores.FirstOrDefaultAsync(x => x.Cnpj == storeEntity.Cnpj, ct);
        if (existingStore != null)
            return existingStore;

        context.Stores.Add(storeEntity);
        return storeEntity;
    }

    public async Task<List<Store>> GetAllAsync(CancellationToken ct)
    {
        return await context.Stores.ToListAsync(ct);
    }

    public async Task<Store?> GetByIdAsync(Guid storeId, CancellationToken ct)
    {
        return await context.Stores.FindAsync([storeId], ct); 
    }

    public async Task<List<Store>> SearchStoresAsync(SearchStoresRequest request, CancellationToken ct)
    {
        var query =
            from s in context.Stores
            where 
                (string.IsNullOrEmpty(request.Name) || s.Name.ILike(request.Name))
                && (request.State == StatesEnum.Empty || s.State == request.State)
                && (string.IsNullOrEmpty(request.CityName) || s.State.StringValue().ILike(request.CityName))
            select s;

        return await query
            .AsNoTracking()
            .Skip(request.PageSize * request.PageIndex)
            .Take(request.PageSize)
            .ToListAsync(ct);
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}