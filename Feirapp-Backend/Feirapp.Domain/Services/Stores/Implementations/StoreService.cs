using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Domain.Services.Stores.Methods.GetStoreById;
using Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;
using Feirapp.Domain.Services.Stores.Methods.SearchStores;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Domain.Services.Utils;

namespace Feirapp.Domain.Services.Stores.Implementations;

public class StoreService(IUnitOfWork uow) : IStoreService
{
    public async Task<Result<bool>> InsertStoreAsync(InsertStoreRequest store, CancellationToken ct)
    {
        await uow.StoreRepository.InsertAsync(store.ToEntity(), ct);
        await uow.SaveChangesAsync(ct);
        return Result<bool>.Ok(true);
    }
    
    public async Task<Result<List<SearchStoresResponse>>> SearchStoresAsync(SearchStoresRequest request, CancellationToken ct)
    {
        var stores = await uow.StoreRepository.SearchStoresAsync(request, ct);
        return Result<List<SearchStoresResponse>>.Ok(stores.ToSearchResponse());
    }
    
    public async Task<Result<GetStoreByIdResponse>> GetStoreByIdAsync(Guid storeId, CancellationToken ct)
    {
        var result = await uow.StoreRepository.GetByIdAsync(storeId, ct);
        return result is null
            ? Result<GetStoreByIdResponse>.Fail("Stores not found.")
            : Result<GetStoreByIdResponse>.Ok(result.ToGetStoreByIdResponse());
    }
}