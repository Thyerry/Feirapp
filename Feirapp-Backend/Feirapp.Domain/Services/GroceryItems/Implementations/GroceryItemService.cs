using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.Cests.Interfaces;
using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Command;
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
    public async Task<List<SearchGroceryItemsResponse>> SearchGroceryItemsAsync(SearchGroceryItemsQuery query, CancellationToken ct)
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

            await InsertGroceryItem(groceryItem, (long) command.Store!.Id!, command.Price, DateTime.Now, ct);

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
            var store = await storeRepository.AddIfNotExistsAsync(s => s.Cnpj == command.Store.Cnpj, command.Store.MapToEntity(), ct);

            var ncms = command.GroceryItems.GroupBy(g => g.NcmCode).Select(item => item.Key).ToList();
            var cests = command.GroceryItems.GroupBy(g => g.CestCode).Select(item => item.Key).ToList();
            
            await ncmRepository.InsertListOfCodesAsync(ncms, ct);
            await cestRepository.InsertListOfCodesAsync(cests, ct);
            
            foreach (var item in command.GroceryItems)
            {
                var insertGroceryItem = item.ToEntity();
                if(insertGroceryItem.Barcode != "SEM GTIN" && insertGroceryItem.Barcode.Length > 13)
                    insertGroceryItem.Barcode = insertGroceryItem.Barcode.Substring(1, 13);
                await InsertGroceryItem(insertGroceryItem, store.Id, item.Price, DateTime.Now, ct);
            }

            await trans.CommitAsync(ct);
        }
        catch (Exception)
        {
            await trans.RollbackAsync(ct);
            throw;
        }
    }

    private async Task InsertGroceryItem(GroceryItem item, long storeId, decimal price, DateTime purchaseDate, CancellationToken ct)
    {
        var itemFromDb = await groceryItemRepository.CheckIfGroceryItemExistsAsync(item, storeId, ct);
        if (itemFromDb == null)
        {
            await groceryItemRepository.InsertAsync(item, ct);
            var priceLog = new PriceLog()
            {
                GroceryItemId = item.Id,
                Barcode = item.Barcode,
                StoreId = storeId,
                Price = price,
                LogDate = purchaseDate,
            };

            await groceryItemRepository.InsertPriceLog(priceLog, ct);
        }
        else
        {
            var lastPriceLog = await groceryItemRepository.GetLastPriceLogAsync(itemFromDb.Id, storeId, ct);
            if (itemFromDb.AltNames != null && itemFromDb.Name != item.Name && itemFromDb.AltNames.Contains(item.Name) == false)
            {
                itemFromDb.AltNames.Add(item.Name);
                await groceryItemRepository.UpdateAsync(itemFromDb, ct);
            }
            if (lastPriceLog == null || (price != lastPriceLog.Price && purchaseDate.Date > lastPriceLog.LogDate.Date))
            {
                var priceLog = new PriceLog()
                {
                    GroceryItemId = itemFromDb.Id,
                    Barcode = item.Barcode,
                    StoreId = storeId,
                    Price = price,
                    LogDate = purchaseDate,
                };

                await groceryItemRepository.InsertPriceLog(priceLog, ct);
            }
        }
    }
}