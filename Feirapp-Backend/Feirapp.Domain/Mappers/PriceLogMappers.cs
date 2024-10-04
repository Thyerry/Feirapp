using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Entities.Entities;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class PriceLogMappers
{
    public static partial PriceLogDto ToDto(this PriceLog priceLogs);
}