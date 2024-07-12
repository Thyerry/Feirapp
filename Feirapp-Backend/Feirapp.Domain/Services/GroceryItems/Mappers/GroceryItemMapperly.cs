using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Entities.Entities;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Services.GroceryItems.Mappers;

[Mapper]
public static partial class GroceryItemMapperly
{
    public static partial GroceryItemByStore ToStoreItem(this GroceryItem model);
    public static partial List<GroceryItemByStore> ToStoreItem(this List<GroceryItem> model);
}