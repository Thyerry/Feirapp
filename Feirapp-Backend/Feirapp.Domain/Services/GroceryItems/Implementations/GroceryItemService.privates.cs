using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Utils;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public partial class GroceryItemService
{
    private async Task<Store> EnsureStoreExistsAsync(InsertStore storeDto, CancellationToken ct)
    {
        var store = await AddIfNotExistsAsync(storeDto.ToEntity(), ct);
        
        if (store.Name == storeDto.Name || (store.AltNames ?? []).Contains(storeDto.Name)) 
            return store;

        store.AltNames = store.AltNames == null 
            ? [storeDto.Name] 
            : store.AltNames.Append(storeDto.Name).ToList();
        
        await uow.StoreRepository.UpdateAsync(store, ct);

        return store;
    }

    private async Task<Store> AddIfNotExistsAsync(Store store, CancellationToken ct)
    {
        var existingStore = await uow.StoreRepository.GetByCnpjAsync(store.Cnpj, ct);
        if (existingStore != null)
        {
            store.Id = existingStore.Id;
            return existingStore;
        }

        await uow.StoreRepository.InsertAsync(store, ct);
        return store;
    }

    private async Task InsertNcmsAndCestsAsync(InsertListOfGroceryItemsCommand command, CancellationToken ct)
    {
        var ncms = command.GroceryItems.Select(g => g.NcmCode).Distinct().ToList();
        var cests = command.GroceryItems.Select(g => g.CestCode).Distinct().ToList();
        await uow.NcmRepository.InsertListOfCodesAsync(ncms, ct);
        if(cests.Count != 0)
            await uow.CestRepository.InsertListOfCodesAsync(cests, ct);
    }

    private async Task InsertGroceryItemsAsync(List<InsertGroceryItem> items, Guid storeId, CancellationToken ct)
    {
        foreach (var itemDto in items)
        {
            var groceryItem = itemDto.ToEntity();
            groceryItem.Barcode = AdjustBarcode(groceryItem.Barcode);
            await InsertGroceryItem(groceryItem, storeId, itemDto.Price, itemDto.PurchaseDate, itemDto.ProductCode, ct);
        }
    }

    private static string AdjustBarcode(string barcode)
    {
        return barcode != "SEM GTIN" && barcode.Length > 13 ? barcode.Substring(1, 13) : barcode;
    }

    private async Task InsertGroceryItem(GroceryItem item, Guid storeId, decimal price, DateTime purchaseDate, string productCode, CancellationToken ct)
    {
        var itemFromDb = await uow.GroceryItemRepository.CheckIfGroceryItemExistsAsync(item, storeId, ct);
        if (itemFromDb == null)
        {
            item.Id = GuidGenerator.Generate();
            await uow.GroceryItemRepository.InsertAsync(item, ct);
        }
        else
        {
            InsertAltName(itemFromDb, item.Name);
            await uow.GroceryItemRepository.UpdateAsync(itemFromDb, ct);
        }

        await InsertOrUpdatePriceLog(itemFromDb ?? item, storeId, price, purchaseDate, productCode, ct);
    }

    private static void InsertAltName(GroceryItem itemFromDb, string newName)
    {
        if (itemFromDb.Name == newName || (itemFromDb.AltNames ?? []).Contains(newName))
            return;
        
        itemFromDb.AltNames = itemFromDb.AltNames == null
            ? [newName]
            : itemFromDb.AltNames.Append(newName).ToList();
    }

    private async Task InsertOrUpdatePriceLog(GroceryItem item, Guid storeId, decimal price, DateTime purchaseDate, string productCode, CancellationToken ct)
    {
        var lastPriceLog = await uow.GroceryItemRepository.GetLastPriceLogAsync(item.Id, storeId, ct);
        if (lastPriceLog == null || (Math.Round(price, 2) != Math.Round(lastPriceLog.Price, 2) && purchaseDate > lastPriceLog.LogDate))
        {
            var priceLog = new PriceLog
            {
                Id = GuidGenerator.Generate(),
                GroceryItemId = item.Id,
                Barcode = item.Barcode,
                StoreId = storeId,
                Price = price,
                LogDate = purchaseDate,
                ProductCode = productCode
            };
            
            await uow.GroceryItemRepository.InsertPriceLog(priceLog, ct);
        }
    }
}