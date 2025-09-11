using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Domain.Services.Stores.Methods.GetStoreById;
using Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;
using Feirapp.Domain.Services.UnitOfWork;

namespace Feirapp.Domain.Services.Stores.Implementations;

public class StoreService(IUnitOfWork uow) : IStoreService
{
    public async Task InsertStoreAsync(InsertStoreRequest store, CancellationToken ct)
    {
        var storeEntity = store.ToEntity();
        
        await uow.StoreRepository.InsertAsync(storeEntity, ct);
        await uow.SaveChangesAsync(ct);
    }

    public async Task<GetStoreByIdResponse?> GetStoreById(Guid storeId, CancellationToken ct)
    {
        var result = await uow.StoreRepository.GetByIdAsync(storeId, ct);
        return result?.ToGetStoreByIdResponse();
    }
}