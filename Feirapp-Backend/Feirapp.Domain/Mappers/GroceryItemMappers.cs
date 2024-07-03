using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Mappers;

public static class GroceryItemMappers
{
    public static ListGroceryItemsResponse ToResponse(this GroceryItemList model)
    {
        return new ListGroceryItemsResponse(
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

    public static List<ListGroceryItemsResponse> ToResponse(this List<GroceryItemList> models)
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
}