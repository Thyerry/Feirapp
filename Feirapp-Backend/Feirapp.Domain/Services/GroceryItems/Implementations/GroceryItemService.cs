using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Domain.Services.GroceryItems.Dtos.Commands;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Mappers;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _groceryItemRepository;
    private readonly IBaseRepository<Store> _storeRepository;
    private readonly IBaseRepository<Ncm> _ncmRepository;
    private readonly IBaseRepository<Cest> _cestRepository;

    public GroceryItemService(IGroceryItemRepository groceryItemRepository, IBaseRepository<Store> storeRepository,
        IBaseRepository<Ncm> ncmRepository, IBaseRepository<Cest> cestRepository)
    {
        _groceryItemRepository = groceryItemRepository;
        _storeRepository = storeRepository;
        _ncmRepository = ncmRepository;
        _cestRepository = cestRepository;
    }

    public async Task<List<GetGroceryItemResponse>> GetAllAsync(CancellationToken ct)
    {
        var groceryItems = await _groceryItemRepository.GetAllAsync(ct);
        return groceryItems.MapToGetAllResponse();
    }

    public async Task InsertBatchAsync(List<InsertGroceryItemCommand> insertCommands,
        CancellationToken ct = default)
    {
        var groceryItems = insertCommands.MapToEntity();

        var store = groceryItems.FirstOrDefault()?.Store;
        var existingNcms = await _ncmRepository.GetAllAsync(ct);
        // var existingCests = await _cestRepository.GetAllAsync(ct);

        var ncms = groceryItems
            .Where(g => string.IsNullOrWhiteSpace(g.NcmCode))
            .Select(x => x.NcmCode)
            .Distinct()
            .Except(existingNcms.Select(s => s.Code))
            .ToList();

        var cests = groceryItems
            .Where(g => string.IsNullOrWhiteSpace(g.CestCode))
            .Select(x => x.CestCode)
            .Distinct()
            // .Except(existingCests.Select(s => s.Code))
            .ToList();

        if (store == null)
            await _storeRepository.AddIfNotExistsAsync(store, x => x.Name == store.Name, ct);

        if (ncms.Count != 0)
            ncms.Select(async s => await _ncmRepository.InsertAsync(new Ncm { Code = s }, ct));

        if (cests.Count != 0)
            cests.Select(async s => await _cestRepository.InsertAsync(new Cest { Code = s }, ct));

        foreach (var item in groceryItems)
        {
            var dbResult = await _groceryItemRepository.GetByBarcodeAndStoreIdAsync(item.Barcode, item.StoreId, ct);

            if (dbResult == null)
            {
                var priceLog = new PriceLog
                {
                    GroceryItemId = item.Id,
                    Price = item.Price,
                    LogDate = item.PurchaseDate
                };
                await _groceryItemRepository.InsertAsync(item, ct);
                await _groceryItemRepository.InsertPriceLogAsync(priceLog, ct);
            }
            else if (item.Price != dbResult.Price)
            {
                var priceLog = new PriceLog
                {
                    GroceryItemId = item.Id,
                    Price = item.Price,
                    LogDate = item.PurchaseDate
                };

                await _groceryItemRepository.UpdateAsync(dbResult, ct);
                await _groceryItemRepository.InsertPriceLogAsync(priceLog, ct);
            }
        }
    }

    //public async Task<List<GroceryItemDto>> GetRandomGroceryItemsAsync(int quantity,
    //    CancellationToken ct)
    //{
    //    return (await _groceryItemRepository.GetRandomGroceryItems(quantity, ct)).ToDtoList();
    //}

    //public async Task<GroceryItemDto> InsertAsync(GroceryItemDto groceryItemDto, CancellationToken ct)
    //{
    //    var validator = new InsertGroceryItemValidator();
    //    await validator.ValidateAndThrowAsync(groceryItemDto, ct);

    //    var groceryItem = groceryItemDto.ToEntity();

    //    return (await _groceryItemRepository.InsertAsync(groceryItem, ct)).ToDto();
    //}

    //public async Task<GroceryItemDto> GetById(long groceryId, CancellationToken ct)
    //{
    //    return (await _groceryItemRepository.GetByIdAsync(groceryId, ct)).ToDto();
    //}

    //public async Task UpdateAsync(GroceryItemDto groceryItemDto, CancellationToken ct)
    //{
    //    var validator = new UpdateGroceryItemValidator();
    //    await validator.ValidateAndThrowAsync(groceryItemDto, ct);

    //    var groceryItem = groceryItemDto.ToEntity();

    //    await _groceryItemRepository.UpdateAsync(groceryItem, ct);
    //}

    //public async Task DeleteAsync(long groceryId, CancellationToken ct)
    //{
    //    await _groceryItemRepository.DeleteAsync(groceryId, ct);
    //}
}