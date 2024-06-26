using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Commands;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Mappers;

public static class GroceryItemMappers
{
    public static List<GroceryItemResponse> MapToGetAllResponse(this List<GroceryItem> entities)
    {
        return entities.Select(x => x.MapToGetAllResponse()).ToList();
    }

    public static GroceryItemResponse MapToGetAllResponse(this GroceryItem entity)
    {
        return new GroceryItemResponse
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
    
    public static StoreResponse MapToStoreResponse(this Store store)
    {
        return new StoreResponse
        (
            Name: store.Name,
            Cnpj: store.Cnpj,
            Cep: store.Cep,
            Street: store.Street,
            StreetNumber: store.StreetNumber,
            Neighborhood: store.Neighborhood,
            CityName: store.CityName,
            State: store.State
        );
    }
    
    public static StoreResponse GetStore(this List<GroceryItem> groceryItems)
    {
        var store = groceryItems.FirstOrDefault()?.Store;
        return store != null ? store.MapToStoreResponse() : new StoreResponse(string.Empty);
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

    public static PriceLogResponse MapToPriceLogDto(this PriceLog log)
    {
        return new PriceLogResponse
        (
            log.Price,
            log.LogDate
        );
    }

    public static List<GroceryItem> MapToEntity(this List<InvoiceGroceryItem> groceryItems)
    {
        return groceryItems.Select(x => x.MapToEntity()).ToList();
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

    public static Store MapToEntity(this InvoiceStore store)
    {
        return new Store
        {
            Name = store.Name,
            Cnpj = store.Cnpj,
            Cep = store.Cep,
            Street = store.Street,
            StreetNumber = store.StreetNumber,
            Neighborhood = store.Neighborhood,
            CityName = store.CityName,
            State = store.State.MapToStatesEnum()
        };
    }
}