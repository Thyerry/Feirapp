using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public partial class GroceryItemService
{
    private async Task<Store> EnsureStoreExistsAsync(StoreDto storeDto, CancellationToken ct)
    {
        var store = await storeRepository.AddIfNotExistsAsync(s => s.Cnpj == storeDto.Cnpj, storeDto.MapToEntity(), ct);
        var storeAltNames = store.AltNames?.Split(",").ToList() ?? [];
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

    private async Task InsertGroceryItem(GroceryItem item, long storeId, decimal price, DateTime purchaseDate, CancellationToken ct)
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

    private async Task InsertOrUpdatePriceLog(GroceryItem item, long storeId, decimal price, DateTime purchaseDate, CancellationToken ct)
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