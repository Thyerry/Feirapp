using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.Cests.Interfaces;
using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Queries;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
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
    public async Task<List<ListGroceryItemsResponse>> ListGroceryItemsAsync(ListGroceryItemsQuery query, CancellationToken ct)
    {
        var entities = await groceryItemRepository.ListGroceryItemsAsync(query, ct);
        return entities.ToResponse();
    }

    public async Task<GetGroceryItemResponse> GetByIdAsync(long id, CancellationToken ct)
    {
        var entity = await groceryItemRepository.GetByIdAsync(id, ct);
        return null;
    }

    public async Task InsertBatchAsync(List<InvoiceGroceryItem> invoiceItems, InvoiceStore invoiceStore,
        CancellationToken ct)
    {
        await using var trans = await groceryItemRepository.BeginTransactionAsync(ct);
        try
        {
            var store = await storeRepository.AddIfNotExistsAsync(s => s.Cnpj == invoiceStore.Cnpj,
                invoiceStore.MapToEntity(), ct);

            var ncms = invoiceItems.GroupBy(g => g.NcmCode).Select(item => item.Key).ToList();
            var cests = invoiceItems.GroupBy(g => g.CestCode).Select(item => item.Key).ToList();

            await ncmRepository.InsertListOfCodesAsync(ncms, ct);
            await cestRepository.InsertListOfCodesAsync(cests, ct);

            foreach (var item in invoiceItems)
            {
                var insertGroceryItem = item.ToEntity();
                var dbResult = await groceryItemRepository.CheckIfGroceryItemExistsAsync(insertGroceryItem, store.Id, ct);

                if (dbResult == null)
                {
                    await groceryItemRepository.InsertAsync(insertGroceryItem, ct);
                    var priceLog = new PriceLog()
                    {
                        GroceryItemId = insertGroceryItem.Id,
                        Barcode = item.Barcode,
                        StoreId = store.Id,
                        Price = item.Price,
                        LogDate = item.PurchaseDate,
                    };

                    await groceryItemRepository.InsertPriceLog(priceLog, ct);
                }
                else
                {
                    var lastPriceLog = await groceryItemRepository.GetLastPriceLogAsync(dbResult.Id, ct);
                    if (lastPriceLog.Price != item.Price)
                    {
                        var priceLog = new PriceLog()
                        {
                            GroceryItemId = dbResult.Id,
                            Barcode = item.Barcode,
                            StoreId = store.Id,
                            Price = item.Price,
                            LogDate = item.PurchaseDate,
                        };

                        await groceryItemRepository.InsertPriceLog(priceLog, ct);
                    }
                }
            }

            await trans.CommitAsync(ct);
        }
        catch (Exception)
        {
            await trans.RollbackAsync(ct);
            throw;
        }

    }
}