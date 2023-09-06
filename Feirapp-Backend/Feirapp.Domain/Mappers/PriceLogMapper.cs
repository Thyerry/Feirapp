using Feirapp.Domain.Models;
using Feirapp.Entities;
using Riok.Mapperly.Abstractions;

namespace Feirapp.Domain.Mappers;

[Mapper]
public partial class PriceLogMapper
{
    public partial PriceLogModel PriceLogToModel(PriceLog priceLog);
    public partial PriceLog ModelToPriceLog(PriceLogModel priceLogModel);
}