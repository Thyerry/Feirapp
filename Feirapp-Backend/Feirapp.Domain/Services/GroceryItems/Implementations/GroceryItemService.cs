using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Domain.Services.GroceryItems.Responses;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public partial class GroceryItemService(IUnitOfWork uow) : IGroceryItemService
{
    public async Task<List<SearchGroceryItemsResponse>> SearchGroceryItemsAsync(SearchGroceryItemsQuery query, CancellationToken ct)
    {
        var entities = await uow.GroceryItemRepository.SearchGroceryItemsAsync(query, ct);
        return entities.ToSearchResponse();
    }

    public async Task<GetGroceryItemByIdResponse?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await uow.GroceryItemRepository.GetByIdAsync(id, ct);
        return entity?.ToGetByIdResponse();
    }

    public async Task<GetGroceryItemFromStoreIdResponse> GetByStoreAsync(Guid storeId, CancellationToken ct)
    {
        var result = await uow.GroceryItemRepository.GetByStoreAsync(storeId, ct);
        return new GetGroceryItemFromStoreIdResponse(result.Store.ToDto(), result.Items.ToStoreItem());
    }

    public async Task<List<SearchGroceryItemsResponse>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct)
    {
        var result = await uow.GroceryItemRepository.GetRandomGroceryItemsAsync(quantity, ct);
        return result.ToSearchResponse().OrderBy(_ => Guid.NewGuid()).ToList();
    }

    public async Task InsertAsync(InsertGroceryItemCommand command, CancellationToken ct)
    {
        var groceryItem = new GroceryItem
        {
            Name = command.Name,
            Description = command.Description,
            ImageUrl = command.ImageUrl,
            Barcode = command.Barcode,
            MeasureUnit = command.MeasureUnit,
        };

        await InsertGroceryItem(groceryItem, command.Store!.Id, command.Price, DateTime.Now, command.ProductCode, ct);
    }

    public async Task InsertListAsync(InsertListOfGroceryItemsCommand command, CancellationToken ct)
    {
        var store = await EnsureStoreExistsAsync(command.Store, ct);
        await InsertNcmsAndCestsAsync(command, ct);
        await InsertGroceryItemsAsync(command.GroceryItems, store.Id, ct);
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