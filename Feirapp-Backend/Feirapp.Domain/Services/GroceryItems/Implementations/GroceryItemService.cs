using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public partial class GroceryItemService(IUnitOfWork uow) : IGroceryItemService
{
    public async Task<List<SearchGroceryItemsResponse>> SearchAsync(SearchGroceryItemsRequest request, CancellationToken ct)
    {
        var entities = await uow.GroceryItemRepository.SearchGroceryItemsAsync(request, ct);
        return entities.ToSearchResponse();
    }

    public async Task<GetGroceryItemByIdResponse?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await uow.GroceryItemRepository.GetByIdAsync(id, ct);
        return entity?.ToResponse();
    }

    public async Task<GetGroceryItemsByStoreIdResponse> GetByStoreAsync(Guid storeId, CancellationToken ct)
    {
        var result = await uow.GroceryItemRepository.GetByStoreAsync(storeId, ct);
        return new GetGroceryItemsByStoreIdResponse
        {
            Store = result.Store.ToResponse(),
            Items = result.Items.ToStoreItem()
        };
    }

    public async Task InsertAsync(InsertGroceryItemsRequest request, CancellationToken ct)
    {
        var store = await ValidateAndRegisterStoreAltNameAsync(request.Store.ToEntity(), ct);
        
        await InsertNcmsAndCestsAsync(request, ct);
        
        foreach (var item in request.GroceryItems)
        {
            item.Barcode = ValidateBarcode(item.Barcode);
            var groceryItem = await InsertGroceryItem(item, store.Id, ct);
            await InsertOrUpdatePriceLog(groceryItem, store.Id, item.Price, item.PurchaseDate, item.ProductCode, ct);
        }
        
        await uow.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid groceryId, CancellationToken ct)
    {
        await uow.GroceryItemRepository.DeleteAsync(groceryId, ct);
        await uow.SaveChangesAsync(ct);
    }
}