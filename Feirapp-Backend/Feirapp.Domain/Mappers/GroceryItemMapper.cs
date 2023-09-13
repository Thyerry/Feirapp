using Feirapp.Domain.Dtos;
using Feirapp.DocumentModels;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class GroceryItemMapper
{
    public static partial GroceryItemDto ToDto(this GroceryItem groceryItem);
    public static partial List<GroceryItemDto> ToDtoList(this List<GroceryItem> groceryItem);
    public static partial List<GroceryItem> ToModelList(this List<GroceryItemDto> groceryItem);
    public static partial GroceryItem ToModel(this GroceryItemDto groceryItemModel);
}