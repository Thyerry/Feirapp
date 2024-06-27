using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Mappers;

public static class GroceryItemMappers
{
    public static List<GetAllGroceryItemsResponse> MapToGetAllResponse(this List<GroceryItem> entities)
    {
        return entities.Select(x => x.MapToGetAllResponse()).ToList();
    }
    public static GetAllGroceryItemsResponse MapToGetAllResponse(this GroceryItem entity)
    {
        return new GetAllGroceryItemsResponse
        (
            entity.Id,
            entity.Name,
            entity.Description!,
            entity.Price,
            entity.ImageUrl!,
            entity.Barcode!,
            entity.LastUpdate,
            entity.PurchaseDate,
            entity.Store.MapToDto(),
            entity.MeasureUnit,
            entity.PriceHistory.Select(x => x.MapToPriceLogDto()).ToList()
        );
    }
    public static List<GroceryItemDto> MapToDto(this List<GroceryItem> entity)
    {
        return entity.Select(g => g.MapToDto()).ToList();
    }
    public static GroceryItemDto MapToDto(this GroceryItem entity)
    {
        return new GroceryItemDto
        (
            entity.Id,
            entity.Name,
            entity.Description!,
            entity.Price,
            entity.ImageUrl!,
            entity.Barcode!,
            entity.LastUpdate,
            entity.PurchaseDate,
            entity.MeasureUnit,
            entity.PriceHistory.Select(x => x.MapToPriceLogDto()).ToList()
        );
    }
    public static GroceryItem MapToEntity(this GroceryItemDto dto)
    {
        return new GroceryItem
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            Barcode = dto.Barcode,
            MeasureUnit = dto.MeasureUnit,
            PurchaseDate = dto.PurchaseDate
        };
    }
    public static List<GroceryItem> MapToEntity(this List<GroceryItemDto> dto)
    {
        return dto.Select(x => x.MapToEntity()).ToList();
    }
    public static GroceryItem MapToEntity(this InvoiceGroceryItem groceryItem)
    {
        return new GroceryItem
        {
            Name = groceryItem.Name,
            Price = groceryItem.Price,
            Barcode = groceryItem.Barcode,
            NcmCode = groceryItem.NcmCode,
            MeasureUnit = groceryItem.MeasureUnit.MapToMeasureUnit(),
            PurchaseDate = groceryItem.PurchaseDate,
            CestCode = groceryItem.CestCode,
        };
    }
    public static List<GroceryItem> MapToEntity(this List<InvoiceGroceryItem> groceryItems)
    {
        return groceryItems.Select(x => x.MapToEntity()).ToList();
    }
    public static StoreDto GetStore(this List<GroceryItem> groceryItems)
    {
        var store = groceryItems.FirstOrDefault()?.Store;
        return store != null ? store.MapToDto() : new StoreDto(string.Empty);
    }
    public static InsertGroceryItemResponse MapToInsertResponse(this GroceryItem entity)
    {
        return new InsertGroceryItemResponse
        (
            entity.Id,
            entity.Name,
            entity.Price,
            entity.Barcode,
            entity.Description!,
            entity.ImageUrl!,
            entity.NcmCode,
            entity.CestCode,
            entity.Store.Name,
            entity.Store.Cnpj,
            entity.Store.Cep,
            entity.Store.Street,
            entity.Store.StreetNumber,
            entity.Store.Neighborhood,
            entity.Store.CityName,
            (StatesEnum)entity.Store.State!
        );
    }
    public static List<InsertGroceryItemResponse> MapToInsertResponse(this List<GroceryItem> entities)
    {
        return entities.Select(x => x.MapToInsertResponse()).ToList();
    }
    public static PriceLogDto MapToPriceLogDto(this PriceLog log)
    {
        return new PriceLogDto
        (
            log.Price,
            log.LogDate
        );
    }
}