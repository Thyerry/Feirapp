using Feirapp.DocumentModels.Documents;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Services.GroceryItems.Mappers;

[Mapper]
public partial class PriceLogMapper
{
    public partial PriceLogDto PriceLogToModel(PriceLog priceLog);

    public partial PriceLog ModelToPriceLog(PriceLogDto priceLogModel);
}