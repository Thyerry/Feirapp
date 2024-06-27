namespace Feirapp.Domain.Services.DataScrapper.Dtos;

public record NcmCestData(NcmDto Ncm, List<CestDto>? Cests = null);