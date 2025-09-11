using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertListOfGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Misc;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class GroceryItemMappers
{
    public static partial List<GetGroceryItemsByStoreGroceryItemDto> ToStoreItem(this List<GroceryItem> model);
    public static partial List<SearchGroceryItemsResponse> ToSearchResponse(this List<SearchGroceryItemsDto> model);
    public static partial GenericGroceryItemDto ToGeneric(this InsertListOfGroceryItemsDto model);
    public static partial GroceryItem ToEntity(this GenericGroceryItemDto model);
    private static string StringAltNames(List<string> altNames) => MapperUtils.StringAltNames(altNames);
    private static List<string> ListAltNames(string nameList) => MapperUtils.ListAltNames(nameList);
    private static MeasureUnitEnum ToMeasureUnitEnum(this string measureUnit) => EnumMappers.ToMeasureUnitEnum(measureUnit);
}