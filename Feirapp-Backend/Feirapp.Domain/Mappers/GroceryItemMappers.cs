using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Mappers;

public static class GroceryItemMappers
{
    public static SearchGroceryItemsResponse ToResponse(this SearchGroceryItemsDto model)
    {
        return new SearchGroceryItemsResponse(
            model.Id,
            model.Name,
            model.Description,
            model.LastPrice,
            model.ImageUrl,
            model.Barcode,
            model.LastUpdate,
            model.MeasureUnit,
            model.StoreId,
            model.StoreName
        );
    }

    public static List<SearchGroceryItemsResponse> ToResponse(this List<SearchGroceryItemsDto> models)
    {
        return models.Select(ToResponse).ToList();
    }

    public static GroceryItem ToEntity(this InvoiceGroceryItem model)
    {
        return new GroceryItem
        {
            Name = model.Name,
            Barcode = model.Barcode,
            MeasureUnit = model.MeasureUnit.MapToMeasureUnit(),
            NcmCode = model.NcmCode,
            CestCode = model.CestCode,
        };
    }
    
    public static List<GroceryItem> ToEntity(this List<InvoiceGroceryItem> models)
    {
        return models.Select(m => m.ToEntity()).ToList();
    }
    
    public static List<GroceryItem> ToEntity(this List<GroceryItemDto> models)
    {
        return models.Select(m => m.ToEntity()).ToList();
    }
    public static GroceryItem ToEntity(this GroceryItemDto model)
    {
        return new GroceryItem
        {
            Name = model.Name,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            Barcode = model.Barcode,
            MeasureUnit = model.MeasureUnit,
        };
    }

    public static GetGroceryItemByIdResponse ToGetByIdResponse(this GroceryItem entity)
    {
        return new GetGroceryItemByIdResponse
        (
            entity.Id,
            entity.Name,
            entity.Description,
            entity.ImageUrl,
            entity.Brand,
            entity.Barcode,
            entity.NcmCode!,
            entity.CestCode!,
            entity.MeasureUnit,
            entity.PriceHistory?.Select(p => p.ToDto()).ToList()!
        );
    }
    
    public static GroceryItemDto ToDto(this GroceryItem entity)
    {
        return new GroceryItemDto
        (
            entity.Id,
            entity.Name,
            entity.Description!,
            entity.ImageUrl!,
            entity.Barcode,
            entity.MeasureUnit,
            entity.PriceHistory?.Select(p => p.ToDto()).ToList()!
        );
    }
    
    public static List<GroceryItemDto> ToDto(this List<GroceryItem> entities)
    {
        return entities.Select(ToDto).ToList();
    }
}