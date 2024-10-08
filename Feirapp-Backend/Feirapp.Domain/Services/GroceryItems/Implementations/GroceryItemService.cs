using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.Cests.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Domain.Services.GroceryItems.Responses;
using Feirapp.Domain.Services.Ncms.Interfaces;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public partial class GroceryItemService(
    IGroceryItemRepository groceryItemRepository,
    IStoreRepository storeRepository,
    INcmRepository ncmRepository,
    ICestRepository cestRepository) : IGroceryItemService
{
    public async Task<List<SearchGroceryItemsResponse>> SearchGroceryItemsAsync(SearchGroceryItemsQuery query, CancellationToken ct)
    {
        var entities = await groceryItemRepository.SearchGroceryItemsAsync(query, ct);
        return entities.ToSearchResponse();
    }

    public async Task<GetGroceryItemByIdResponse?> GetByIdAsync(long id, CancellationToken ct)
    {
        var entity = await groceryItemRepository.GetByIdAsync(id, ct);
        return entity?.ToGetByIdResponse();
    }

    public async Task<GetGroceryItemFromStoreIdResponse> GetByStoreAsync(long storeId, CancellationToken ct)
    {
        var result = await groceryItemRepository.GetByStoreAsync(storeId, ct);
        return new GetGroceryItemFromStoreIdResponse(result.Store?.ToDto(), result.Items.ToStoreItem());
    }

    public async Task<List<SearchGroceryItemsResponse>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct)
    {
        var result = await groceryItemRepository.GetRandomGroceryItemsAsync(quantity, ct);
        return result.ToSearchResponse().OrderBy(p => Guid.NewGuid()).ToList();
    }

    public async Task InsertAsync(InsertGroceryItemCommand command, CancellationToken ct)
    {
        await using var trans = await groceryItemRepository.BeginTransactionAsync(ct);
        try
        {
            var groceryItem = new GroceryItem
            {
                Name = command.Name,
                Description = command.Description,
                ImageUrl = command.ImageUrl,
                Barcode = command.Barcode,
                MeasureUnit = command.MeasureUnit,
            };

            await InsertGroceryItem(groceryItem, (long)command.Store!.Id!, command.Price, DateTime.Now, ct);

            await trans.CommitAsync(ct);
        }
        catch (Exception)
        {
            await trans.RollbackAsync(ct);
            throw;
        }
    }

    public async Task InsertListAsync(InsertListOfGroceryItemsCommand command, CancellationToken ct)
    {
        await using var trans = await groceryItemRepository.BeginTransactionAsync(ct);
        try
        {
            var store = await EnsureStoreExistsAsync(command.Store, ct);
            await InsertNcmsAndCestsAsync(command, ct); 
            await InsertGroceryItemsAsync(command.GroceryItems, store.Id, ct);
            await trans.CommitAsync(ct);
        }
        catch (Exception)
        {
            await trans.RollbackAsync(ct);
            throw;
        }
    }

    public async Task UpdateAsync(UpdateGroceryItemCommand groceryItem, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(long groceryId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}