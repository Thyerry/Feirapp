using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Responses;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class GroceryItemMappers
{
    public static partial GroceryItemByStore ToStoreItem(this GroceryItem model);
    public static partial List<GroceryItemByStore> ToStoreItem(this List<GroceryItem> model);
    public static partial SearchGroceryItemsResponse ToSearchResponse(this SearchGroceryItemsDto model);
    public static partial List<SearchGroceryItemsResponse> ToSearchResponse(this List<SearchGroceryItemsDto> model);
    public static partial GroceryItem ToEntity(this InsertGroceryItem model);
    public static partial List<GroceryItem> ToEntity(this GroceryItemDto model);
    public static partial GetGroceryItemByIdResponse ToGetByIdResponse(this GroceryItem model);
    public static partial List<GetGroceryItemByIdResponse> ToGetByIdResponse(this List<GroceryItem> model);
    private static string StringAltNames(List<string> altNames) => MapperUtils.StringAltNames(altNames);
    private static List<string> ListAltNames(string nameList) => MapperUtils.ListAltNames(nameList);
    private static MeasureUnitEnum ToMeasureUnitEnum(this string measureUnit) => EnumMappers.ToMeasureUnitEnum(measureUnit);
}