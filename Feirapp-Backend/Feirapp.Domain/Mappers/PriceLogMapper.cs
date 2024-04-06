using Feirapp.DocumentModels.Documents;
using Feirapp.Domain.Dtos;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public partial class PriceLogMapper
{
    public partial PriceLogDto PriceLogToModel(PriceLog priceLog);

    public partial PriceLog ModelToPriceLog(PriceLogDto priceLogModel);
}