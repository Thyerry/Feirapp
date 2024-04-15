using Feirapp.Domain.Services.DataScrapper.Dtos;

namespace Feirapp.Domain.Services.DataScrapper.Interfaces;

public interface INcmCestDataScrapper
{
    /// <summary>
    /// Scrap the data from the NCM/CEST page.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the NCM/CEST data.</returns>
    Task UpdateNcmAndCestsDetails(CancellationToken ct);
}