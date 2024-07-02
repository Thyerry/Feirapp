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
        return entities.Select(entity => entity.MapToGetAllResponse()).ToList();
    }
    public static GetAllGroceryItemsResponse MapToGetAllResponse(this GroceryItem entity)
    {
        return new GetAllGroceryItemsResponse(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Price,
            entity.ImageUrl,
            entity.Barcode,
            entity.LastUpdate,
            entity.LastPurchaseDate,
            entity.MeasureUnit
        );
    }
}