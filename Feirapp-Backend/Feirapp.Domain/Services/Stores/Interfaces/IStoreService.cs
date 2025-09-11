using Feirapp.Domain.Services.Stores.Methods.GetStoreById;
using Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;

namespace Feirapp.Domain.Services.Stores.Interfaces;

public interface IStoreService
{
    Task InsertStoreAsync(InsertStoreRequest store, CancellationToken ct);
    Task<GetStoreByIdResponse?> GetStoreById(Guid storeId, CancellationToken ct);
}