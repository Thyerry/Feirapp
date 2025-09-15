namespace Feirapp.Domain.Services.DataScrapper.Interfaces;

public interface INcmCestDataScrapper
{
    Task UpdateNcmAndCestsDetailsAsync(CancellationToken ct);
}