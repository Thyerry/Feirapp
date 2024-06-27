using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Command;
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

    public async Task<List<GetAllGroceryItemsResponse>> GetAllAsync(CancellationToken ct)
    {
        var groceryItems = await _groceryItemRepository.GetAllAsync(ct);
        return groceryItems.MapToGetAllResponse();
    }

    public async Task InsertBatchAsync(List<InvoiceGroceryItem> items, InvoiceStore invoiceStore, CancellationToken ct)
    {
        await using var trans = await _groceryItemRepository.BeginTransactionAsync(ct);
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

            await trans.CommitAsync(ct);
        }
        catch (Exception)
        {
            await trans.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<GetGroceryItemResponse> GetByStoreAsync(long storeId, CancellationToken ct)
    {
        var groceryItems = await _groceryItemRepository.GetByStoreAsync(storeId, ct);
        return new GetGroceryItemResponse(groceryItems.GetStore(), groceryItems.MapToDto());
    }

    public async Task<List<GetAllGroceryItemsResponse>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct)
    {
        var groceryItems = await _groceryItemRepository.GetRandomGroceryItemsAsync(quantity, ct);
        return groceryItems.MapToGetAllResponse();
    }

    public async Task<GroceryItemDto> InsertAsync(GroceryItemDto groceryItem, CancellationToken ct)
    {
        var result = await _groceryItemRepository.InsertAsync(groceryItem.MapToEntity(), ct);
        return result.MapToDto();
    }

    public async Task<GroceryItemDto> GetByIdAsync(long id, CancellationToken ct)
    {
        var result = await _groceryItemRepository.GetByIdAsync(id, ct);
        return result.MapToDto();
    }

    public async Task UpdateAsync(UpdateGroceryItemCommand command, CancellationToken ct)
    {
        command.Validate();
        var groceryItem =
            await _groceryItemRepository.GetByBarcodeAndStoreIdAsync(command.Barcode, command.StoreId, ct);

        if (groceryItem is null)
            throw new InvalidOperationException("Grocery Item Not Found");

        var hasChanged = UpdateGroceryItemFields(command, groceryItem);

        if (!hasChanged)
            throw new InvalidOperationException("No changes were made to the Grocery Item.");

        await _groceryItemRepository.UpdateAsync(groceryItem, ct);
    }

    private static bool UpdateGroceryItemFields(UpdateGroceryItemCommand command, GroceryItem groceryItem)
    {
        var hasChanged = false;
        if (!string.IsNullOrWhiteSpace(command.Brand) && groceryItem.Brand != command.Brand)
        {
            groceryItem.Brand = command.Brand;
            hasChanged = true;
        }

        if (!string.IsNullOrWhiteSpace(command.Description) && groceryItem.Description != command.Description)
        {
            groceryItem.Description = command.Description;
            hasChanged = true;
        }

        if (!string.IsNullOrWhiteSpace(command.ImageUrl) && groceryItem.ImageUrl != command.ImageUrl)
        {
            groceryItem.ImageUrl = command.ImageUrl;
            hasChanged = true;
        }

        if (groceryItem.Price != command.Price)
        {
            groceryItem.Price = command.Price;
            hasChanged = true;
        }

        if (groceryItem.PurchaseDate.Date <= command.PurchaseDate.Date)
        {
            groceryItem.PurchaseDate = command.PurchaseDate;
            hasChanged = true;
        }

        return hasChanged;
    }
}