using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertListOfGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Misc;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Utils;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public partial class GroceryItemService
{
    private async Task<Store> EnsureStoreExistsAsync(Store storeToCheck, CancellationToken ct)
    {
        var store = await AddIfNotExistsAsync(storeToCheck, ct);
        
        if (store.Name == storeToCheck.Name || (store.AltNames ?? []).Contains(storeToCheck.Name)) 
            return store;

        store.AltNames = store.AltNames == null 
            ? [storeToCheck.Name] 
            : store.AltNames.Append(storeToCheck.Name).ToList();
        
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

        store.Id = GuidGenerator.Generate();
        await uow.StoreRepository.InsertAsync(store, ct);
        return store;
    }

    private async Task InsertNcmsAndCestsAsync(InsertListOfGroceryItemsRequest request, CancellationToken ct)
    {
        var ncms = request.GroceryItems.Select(g => g.NcmCode).Distinct().ToList();
        var cests = request.GroceryItems.Select(g => g.CestCode).Distinct().ToList();
        await uow.NcmRepository.InsertListOfCodesAsync(ncms, ct);
        if(cests.Count != 0)
            await uow.CestRepository.InsertListOfCodesAsync(cests, ct);
    }

    private async Task InsertGroceryItemsAsync(List<InsertListOfGroceryItemsDto> items, Guid storeId, CancellationToken ct)
    {
        foreach (var itemDto in items)
        {
            itemDto.Barcode = ValidateBarcode(itemDto.Barcode);
            await InsertGroceryItem(itemDto.ToGeneric(), storeId, itemDto.Price, itemDto.PurchaseDate, itemDto.ProductCode, ct);
        }
    }

    private string ValidateBarcode(string barcode)
    {
        return barcode != "SEM GTIN" && barcode.Length > 13 ? barcode.Substring(1, 13) : barcode;
    }
    
    private async Task InsertGroceryItem(GenericGroceryItemDto item, Guid storeId, decimal price, DateTime purchaseDate, string productCode, CancellationToken ct)
    {
        var toInsert = item.ToEntity();
        var itemFromDb = await uow.GroceryItemRepository.CheckIfGroceryItemExistsAsync(item.Barcode, item.ProductCode, storeId, ct);
        if (itemFromDb == null)
        {
            toInsert.Id = GuidGenerator.Generate();
            await uow.GroceryItemRepository.InsertAsync(toInsert, ct);
        }
        else
        {
            InsertAltName(itemFromDb, item.Name);
        }

        await InsertOrUpdatePriceLog(itemFromDb ?? toInsert, storeId, price, purchaseDate, productCode, ct);
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