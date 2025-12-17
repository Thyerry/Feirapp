using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Domain.Services.Utils;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public partial class GroceryItemService(IUnitOfWork uow) : IGroceryItemService
{
    public async Task<Result<List<SearchGroceryItemsResponse>>> SearchAsync(SearchGroceryItemsRequest request, CancellationToken ct)
    {
        var entities = await uow.GroceryItemRepository.SearchGroceryItemsAsync(request, ct);
        return Result<List<SearchGroceryItemsResponse>>.Ok(entities.ToSearchResponse());
    }

    public async Task<Result<GetGroceryItemByIdResponse>> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await uow.GroceryItemRepository.GetByIdAsync(id, ct);
        return entity is null
            ? Result<GetGroceryItemByIdResponse>.Fail("Grocery item not found")
            : Result<GetGroceryItemByIdResponse>.Ok(entity.ToResponse());
    }

    public async Task<Result<GetGroceryItemsByStoreIdResponse>> GetByStoreAsync(Guid storeId, CancellationToken ct)
    {
        var result = await uow.GroceryItemRepository.GetByStoreAsync(storeId, ct);
        var response = new GetGroceryItemsByStoreIdResponse
        {
            Store = result.Store.ToResponse(),
            Items = result.Items.ToStoreItem()
        };

        return response.Items.Count == 0 
            ? Result<GetGroceryItemsByStoreIdResponse>.Fail("Store not found") 
            : Result<GetGroceryItemsByStoreIdResponse>.Ok(response);
    }

    public async Task<Result<int>> InsertAsync(InsertGroceryItemsRequest request, CancellationToken ct)
    {
        var store = await ValidateAndRegisterStoreAltNameAsync(request.Store.ToEntity(), ct);
        
        await InsertNcmsAndCestsAsync(request, ct);
        
        var processed = 0;
        foreach (var item in request.GroceryItems)
        {
            item.Barcode = ValidateBarcode(item.Barcode);
            var groceryItem = await InsertGroceryItem(item, store.Id, ct);
            await InsertOrUpdatePriceLog(groceryItem, store.Id, item.Price, item.PurchaseDate, item.ProductCode, ct);
            processed++;
        }
        
        await uow.SaveChangesAsync(ct);
        return Result<int>.Ok(processed, $"Imported {processed} items");
    }

    public async Task<Result<bool>> DeleteAsync(Guid groceryId, CancellationToken ct)
    {
        await uow.GroceryItemRepository.DeleteAsync(groceryId, ct);
        await uow.SaveChangesAsync(ct);
        return Result<bool>.Ok(true);
    }
}