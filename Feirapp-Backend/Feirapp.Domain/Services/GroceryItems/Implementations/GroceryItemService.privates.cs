using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Misc;
using Feirapp.Domain.Services.Utils;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public partial class GroceryItemService
{
    private async Task<Store> ValidateAndRegisterStoreAltNameAsync(Store storeToCheck, CancellationToken ct)
    {
        storeToCheck.Id = GuidGenerator.Generate();
        var store = await uow.StoreRepository.AddIfNotExistsAsync(x => x.Cnpj == storeToCheck.Cnpj, storeToCheck, ct);
        
        if (store.Name == storeToCheck.Name || (store.AltNames ?? []).Contains(storeToCheck.Name)) 
            return store;

        store.AltNames = store.AltNames == null 
            ? [storeToCheck.Name] 
            : store.AltNames.Append(storeToCheck.Name).ToList();
        
        return store;
    }

    private async Task InsertNcmsAndCestsAsync(InsertGroceryItemsRequest request, CancellationToken ct)
    {
        var ncms = request.GroceryItems.Select(g => g.NcmCode).Distinct().ToList();
        var cests = request.GroceryItems.Select(g => g.CestCode).Distinct().ToList();
        await uow.NcmRepository.InsertListOfCodesAsync(ncms, ct);
        if(cests.Count != 0)
            await uow.CestRepository.InsertListOfCodesAsync(cests, ct);
    }

    private static string ValidateBarcode(string barcode)
    {
        return barcode != "SEM GTIN" && barcode.Length > 13 ? barcode.Substring(1, 13) : barcode;
    }
    
    private async Task<GroceryItem> InsertGroceryItem(InsertGroceryItemsDto item, Guid storeId, decimal price, DateTime purchaseDate, string productCode, CancellationToken ct)
    {
        var toInsert = item.ToEntity();
        var itemFromDb = await uow.GroceryItemRepository.CheckIfGroceryItemExistsAsync(item.Barcode, item.ProductCode, storeId, ct);
        if (itemFromDb == null)
        {
            toInsert.Id = GuidGenerator.Generate();
            await uow.GroceryItemRepository.InsertAsync(toInsert, ct);
            return toInsert;
        }
        
        if (itemFromDb.Name == item.Name || (itemFromDb.AltNames ?? []).Contains(item.Name))
            return itemFromDb;
    
        itemFromDb.AltNames = itemFromDb.AltNames == null
            ? [item.Name]
            : itemFromDb.AltNames.Append(item.Name).ToList();
        
        return itemFromDb;
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