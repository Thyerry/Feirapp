using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Domain.Services.Stores.Methods.GetStoreById;
using Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;
using Feirapp.Domain.Services.Stores.Methods.SearchStores;
using Feirapp.Domain.Services.UnitOfWork;

namespace Feirapp.Domain.Services.Stores.Implementations;

public class StoreService(IUnitOfWork uow) : IStoreService
{
    public async Task<List<SearchStoresResponse>> SearchStoresAsync(SearchStoresRequest request, CancellationToken ct)
    {
        var stores = await uow.StoreRepository.SearchStoresAsync(request, ct);
        return stores.ToSearchResponse();
    }
    
    public async Task<GetStoreByIdResponse?> GetStoreById(Guid storeId, CancellationToken ct)
    {
        var result = await uow.StoreRepository.GetByIdAsync(storeId, ct);
        return result?.ToGetStoreByIdResponse();
    }
    
    public async Task InsertStoreAsync(InsertStoreRequest store, CancellationToken ct)
    {
        var storeEntity = store.ToEntity();
        
        await uow.StoreRepository.InsertAsync(storeEntity, ct);
        await uow.SaveChangesAsync(ct);
    }
}