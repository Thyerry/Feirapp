using Feirapp.DocumentModels.Documents;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Services.GroceryItems.Mappers;

[Mapper]
public static partial class GroceryItemMapper
{
    public static partial GroceryItemDto ToDto(this GroceryItem groceryItem);

    public static partial List<GroceryItemDto> ToDtoList(this List<GroceryItem> groceryItem);

    public static partial List<GroceryItem> ToModelList(this List<GroceryItemDto> groceryItem);

    public static partial GroceryItem ToModel(this GroceryItemDto groceryItemModel);

    public static partial PriceLogDto PriceLogToModel(PriceLog priceLog);

    public static partial PriceLog ModelToPriceLog(PriceLogDto priceLogModel);
}