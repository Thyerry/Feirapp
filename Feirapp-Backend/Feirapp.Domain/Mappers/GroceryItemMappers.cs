using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class GroceryItemMappers
{
    public static partial List<GetGroceryItemsByStoreGroceryItemDto> ToStoreItem(this List<GroceryItem> model);
    public static partial GetGroceryItemByIdResponse ToResponse(this GetGroceryItemByIdDto model);
    public static partial List<SearchGroceryItemsResponse> ToSearchResponse(this List<SearchGroceryItemsDto> model);
    public static partial GroceryItem ToEntity(this InsertGroceryItemsDto model);
    private static MeasureUnitEnum ToMeasureUnitEnum(this string measureUnit) => EnumMappers.ToMeasureUnitEnum(measureUnit);
}