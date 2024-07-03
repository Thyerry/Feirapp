using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Mappers;

public static class PriceLogMappers
{
    public static PriceLogDto ToDto(this PriceLog priceLog)
    {
        return new PriceLogDto(priceLog.Price, priceLog.LogDate, priceLog.Store.MapToDto());
    }
}