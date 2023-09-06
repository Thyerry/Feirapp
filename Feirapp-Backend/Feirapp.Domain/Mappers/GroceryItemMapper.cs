using Feirapp.Domain.Models;
using Feirapp.Entities;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class GroceryItemMapper
{
    public static partial GroceryItemModel ToModel(this GroceryItem groceryItem);
    public static partial List<GroceryItemModel> ToModelList(this List<GroceryItem> groceryItem);
    public static partial GroceryItem ToEntity(this GroceryItemModel groceryItemModel);
}