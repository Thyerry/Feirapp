using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Domain.Services.GroceryItems.Dtos.Commands;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Mappers;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _groceryItemRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IBaseRepository<Ncm> _ncmRepository;
    private readonly IBaseRepository<Cest> _cestRepository;

    public GroceryItemService(
        IGroceryItemRepository groceryItemRepository,
        IStoreRepository storeRepository,
        IBaseRepository<Ncm> ncmRepository,
        IBaseRepository<Cest> cestRepository)
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
        var storeToInsert = groceryItems.FirstOrDefault(g => g.Store != null)?.Store;
        await _storeRepository.AddIfNotExistsAsync(storeToInsert, x => x.Cnpj == storeToInsert.Cnpj, ct);
        var storeId = (await _storeRepository.GetByCnpjAsync(storeToInsert.Cnpj, ct)) is { } store ? store.Id : 0;
        foreach (var item in groceryItems)
        {
            var dbResult = await _groceryItemRepository.GetByBarcodeAndStoreIdAsync(item.Barcode, storeId, ct);

            if (dbResult == null)
            {
                var newNcm = new Ncm { Code = item.NcmCode, };
                await _ncmRepository.AddIfNotExistsAsync(
                    newNcm,
                    ncm => ncm.Code == item.NcmCode,
                    ct);
                
                var newCest = new Cest { Code = item.CestCode, };
                await _cestRepository.AddIfNotExistsAsync(
                    newCest,
                    cest => cest.Code == item.CestCode,
                    ct);

                await _groceryItemRepository.InsertAsync(item, ct);
                
                var priceLog = new PriceLog
                {
                    GroceryItemId = item.Id,
                    Price = item.Price,
                    LogDate = item.PurchaseDate
                };
                
                await _groceryItemRepository.InsertPriceLogAsync(priceLog, ct);
            }
            // else if (item.Price != dbResult.Price)
            // {
            //     // TODO: Implementar lógica para atualizar o preço do item
            //     var priceLog = new PriceLog
            //     {
            //         GroceryItemId = item.Id,
            //         Price = item.Price,
            //         LogDate = item.PurchaseDate
            //     };
            //     var lastPriceLog = await _groceryItemRepository.GetLastPriceLogAsync(dbResult.Id, ct);
            //     await _groceryItemRepository.UpdateAsync(dbResult, ct);
            //     await _groceryItemRepository.InsertPriceLogAsync(priceLog, ct);
            // }
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