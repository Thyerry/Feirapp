using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItem;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertListOfGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.UpdateGroceryItemCommand;
using Feirapp.Domain.Services.UnitOfWork;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public partial class GroceryItemService(IUnitOfWork uow) : IGroceryItemService
{
    public async Task<List<SearchGroceryItemsResponse>> SearchGroceryItemsAsync(SearchGroceryItemsRequest request, CancellationToken ct)
    {
        var entities = await uow.GroceryItemRepository.SearchGroceryItemsAsync(request, ct);
        return entities.ToSearchResponse();
    }

    public async Task<GetGroceryItemByIdResponse?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await uow.GroceryItemRepository.GetByIdAsync(id, ct);
        return entity?.ToResponse();
    }

    public async Task<GetGroceryItemFromStoreIdResponse> GetByStoreAsync(Guid storeId, CancellationToken ct)
    {
        var result = await uow.GroceryItemRepository.GetByStoreAsync(storeId, ct);
        return new GetGroceryItemFromStoreIdResponse(result.Store.ToResponse(), result.Items.ToStoreItem());
    }

    public async Task<List<SearchGroceryItemsResponse>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct)
    {
        var result = await uow.GroceryItemRepository.GetRandomGroceryItemsAsync(quantity, ct);
        return result.ToSearchResponse().OrderBy(_ => Guid.NewGuid()).ToList();
    }

    public async Task InsertAsync(InsertGroceryItemRequest request, CancellationToken ct)
    {
        // var groceryItem = new InsertGroceryItem()
        // {
        //     Name = request.Name,
        //     Description = request.Description ?? string.Empty,
        //     ImageUrl = request.ImageUrl ?? string.Empty,
        //     Barcode = request.Barcode,
        //     MeasureUnit = request.MeasureUnit.StringValue(),
        // };
        //
        // if (request.Store != null)
        // {
        //     var store = await EnsureStoreExistsAsync(request.Store, ct);
        // }
        //
        // await InsertGroceryItem(groceryItem, request.Store!.Id, request.Price, DateTime.Now, request.ProductCode, ct);
        // await uow.SaveChangesAsync(ct);
    }

    public async Task InsertListAsync(InsertListOfGroceryItemsRequest request, CancellationToken ct)
    {
        var store = await EnsureStoreExistsAsync(request.Store.ToEntity(), ct);
        await InsertNcmsAndCestsAsync(request, ct);
        await InsertGroceryItemsAsync(request.GroceryItems, store.Id, ct);
        await uow.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(UpdateGroceryItemCommand groceryItem, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid groceryId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}