using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Commands;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.Ncms.Interfaces;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _groceryItemRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly INcmRepository _ncmRepository;
    private readonly IBaseRepository<Cest> _cestRepository;

    public GroceryItemService(
        IGroceryItemRepository groceryItemRepository,
        IStoreRepository storeRepository,
        INcmRepository ncmRepository,
        IBaseRepository<Cest> cestRepository)
    {
        _groceryItemRepository = groceryItemRepository;
        _storeRepository = storeRepository;
        _ncmRepository = ncmRepository;
        _cestRepository = cestRepository;
    }

    public async Task<GetGroceryItemResponse> GetAllAsync(CancellationToken ct)
    {
        var groceryItems = await _groceryItemRepository.GetAllAsync(ct);
        return new GetGroceryItemResponse(groceryItems.GetStore(), groceryItems.MapToGetAllResponse());
    }

    public async Task InsertBatchAsync(List<InvoiceGroceryItem> items, InvoiceStore invoiceStore, CancellationToken ct)
    {
        try
        {
            var groceryItems = items.MapToEntity();
            var storeToInsert = invoiceStore.MapToEntity();
            await _storeRepository.AddIfNotExistsAsync(storeToInsert, x => x.Cnpj == storeToInsert.Cnpj, ct);
            var storeId = await _storeRepository.GetByCnpjAsync(storeToInsert.Cnpj, ct) is { } store ? store.Id : 0;

            foreach (var item in groceryItems)
            {
                var dbResult = await _groceryItemRepository.GetByBarcodeAndStoreIdAsync(item.Barcode, storeId, ct);
                item.LastUpdate = DateTime.Now;

                if (dbResult == null)
                {
                    await _ncmRepository.AddIfNotExistsAsync(
                        new Ncm { Code = item.NcmCode, LastUpdate = DateTime.Now },
                        ncm => ncm.Code == item.NcmCode,
                        ct);

                    await _cestRepository.AddIfNotExistsAsync(
                        new Cest { Code = item.CestCode, },
                        cest => cest.Code == item.CestCode,
                        ct);

                    item.StoreId = storeId;
                    var insertedItem = await _groceryItemRepository.InsertAsync(item, ct);

                    var priceLog = new PriceLog
                    {
                        GroceryItemId = insertedItem.Id,
                        Price = item.Price,
                        LogDate = item.PurchaseDate
                    };

                    await _groceryItemRepository.InsertPriceLogAsync(priceLog, ct);
                }
                else if (item.Price != dbResult.Price)
                {
                    item.LastUpdate = DateTime.Now;
                    await _groceryItemRepository.UpdateAsync(dbResult, ct);
                    var priceLog = new PriceLog
                    {
                        GroceryItemId = item.Id,
                        Price = item.Price,
                        LogDate = item.PurchaseDate
                    };
                    await _groceryItemRepository.InsertPriceLogAsync(priceLog, ct);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<GetGroceryItemResponse> GetFromStoreAsync(long storeId, CancellationToken ct)
    {
        var groceryItems = await _groceryItemRepository.GetByStoreIdAsync(storeId, ct);
        return new GetGroceryItemResponse(groceryItems.GetStore(), groceryItems.MapToGetAllResponse());
    }
}