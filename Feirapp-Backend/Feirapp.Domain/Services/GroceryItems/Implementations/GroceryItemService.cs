using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.Cests.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Domain.Services.GroceryItems.Responses;
using Feirapp.Domain.Services.Ncms.Interfaces;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public sealed class GroceryItemService(
    IGroceryItemRepository groceryItemRepository,
    IStoreRepository storeRepository,
    INcmRepository ncmRepository,
    ICestRepository cestRepository)
    : IGroceryItemService
{
    public async Task<List<SearchGroceryItemsResponse>> SearchGroceryItemsAsync(SearchGroceryItemsQuery query,
        CancellationToken ct)
    {
        var entities = await groceryItemRepository.SearchGroceryItemsAsync(query, ct);
        return entities.ToResponse();
    }

    public async Task<GetGroceryItemByIdResponse?> GetByIdAsync(long id, CancellationToken ct)
    {
        var entity = await groceryItemRepository.GetByIdAsync(id, ct);
        return entity?.ToGetByIdResponse();
    }

    public async Task<GetGroceryItemFromStoreIdResponse> GetByStoreAsync(long storeId, CancellationToken ct)
    {
        var result = await groceryItemRepository.GetByStoreAsync(storeId, ct);
        return new GetGroceryItemFromStoreIdResponse(result.Store.MapToDto(), result.Items.ToDto());
    }

    public async Task<List<SearchGroceryItemsResponse>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct)
    {
        var result = await groceryItemRepository.GetRandomGroceryItemsAsync(quantity, ct);
        return result.ToResponse().OrderBy(p => Guid.NewGuid()).ToList();
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

    private async Task<Store> EnsureStoreExistsAsync(StoreDto storeDto, CancellationToken ct)
    {
        var store = await storeRepository.AddIfNotExistsAsync(s => s.Cnpj == storeDto.Cnpj, storeDto.MapToEntity(), ct);
        var storeAltNames = store.AltNames?.Split(",").ToList() ?? new List<string>();
        if (store.Name != storeDto.Name && !storeAltNames.Contains(storeDto.Name))
        {
            storeAltNames.Add(storeDto.Name);
            store.AltNames = string.Join(',', storeAltNames);
            await storeRepository.UpdateAsync(store, ct);
        }

        return store;
    }

    private async Task InsertNcmsAndCestsAsync(InsertListOfGroceryItemsCommand command, CancellationToken ct)
    {
        var ncms = command.GroceryItems.Select(g => g.NcmCode).Distinct().ToList();
        var cests = command.GroceryItems.Select(g => g.CestCode).Distinct().ToList();
        await ncmRepository.InsertListOfCodesAsync(ncms, ct);
        await cestRepository.InsertListOfCodesAsync(cests, ct);
    }

    private async Task InsertGroceryItemsAsync(List<InsertGroceryItem> items, long storeId, CancellationToken ct)
    {
        foreach (var itemDto in items)
        {
            var groceryItem = itemDto.ToEntity();
            groceryItem.Barcode = AdjustBarcode(groceryItem.Barcode);
            await InsertGroceryItem(groceryItem, storeId, itemDto.Price, itemDto.PurchaseDate, ct);
        }
    }

    private string AdjustBarcode(string barcode)
    {
        return barcode != "SEM GTIN" && barcode.Length > 13 ? barcode.Substring(1, 13) : barcode;
    }

    private async Task InsertGroceryItem(GroceryItem item, long storeId, decimal price, DateTime purchaseDate,
        CancellationToken ct)
    {
        var itemFromDb = await groceryItemRepository.CheckIfGroceryItemExistsAsync(item, storeId, ct);
        if (itemFromDb == null)
            await groceryItemRepository.InsertAsync(item, ct);

        else
        {
            UpdateItemAltNamesIfNeeded(itemFromDb, item.Name);
            await groceryItemRepository.UpdateAsync(itemFromDb, ct);
        }

        await InsertOrUpdatePriceLog(itemFromDb ?? item, storeId, price, purchaseDate, ct);
    }

    private void UpdateItemAltNamesIfNeeded(GroceryItem itemFromDb, string newName)
    {
        var altNames = itemFromDb.AltNames?.Split(',').ToList() ?? new List<string>();
        if (itemFromDb.Name != newName && !altNames.Contains(newName))
        {
            altNames.Add(newName);
            itemFromDb.AltNames = string.Join(',', altNames);
        }
    }

    private async Task InsertOrUpdatePriceLog(GroceryItem item, long storeId, decimal price, DateTime purchaseDate,
        CancellationToken ct)
    {
        var lastPriceLog = await groceryItemRepository.GetLastPriceLogAsync(item.Id, storeId, ct);
        if (lastPriceLog == null || (Math.Round(price, 2) != Math.Round(lastPriceLog.Price, 2) &&
                                     purchaseDate.Ticks > lastPriceLog.LogDate.Ticks))
        {
            var priceLog = new PriceLog
            {
                GroceryItemId = item.Id,
                Barcode = item.Barcode,
                StoreId = storeId,
                Price = price,
                LogDate = purchaseDate,
            };
            await groceryItemRepository.InsertPriceLog(priceLog, ct);
        }
    }
}