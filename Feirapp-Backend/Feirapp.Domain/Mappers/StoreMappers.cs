using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Entities.Entities;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public static partial class StoreMappers
{
    public static partial Store ToEntity(this InsertStore command);
    public static partial Store ToEntity(this InvoiceStore command);
    public static partial Store ToEntity(this StoreDto dto);
    public static partial List<Store> ToEntityList(this List<StoreDto> dto);
    public static partial StoreDto ToDto(this Store entity);
    public static partial List<StoreDto> ToDtoList(this List<Store> entity);
}