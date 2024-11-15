using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

/// <summary>
/// Controller for handling operations related to Import Grocery Items.
/// </summary>
[ApiController]
[Route("api/import")]
public class ImportController : ControllerBase
{
    private readonly IInvoiceReaderService _invoiceReaderService;
    private readonly INcmCestDataScrapper _ncmCestDataScrapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportController"/> class.
    /// </summary>
    /// <param name="invoiceReaderService">The service to read invoice data.</param>
    /// <param name="ncmCestDataScrapper"></param>
    public ImportController(IInvoiceReaderService invoiceReaderService, INcmCestDataScrapper ncmCestDataScrapper)
    {
        _invoiceReaderService = invoiceReaderService;
        _ncmCestDataScrapper = ncmCestDataScrapper;
    }

    [HttpPut("update-ncm")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    public async Task<IActionResult> ImportFromNcmCest(CancellationToken ct)
    {
        await _ncmCestDataScrapper.UpdateNcmAndCestsDetails(ct);
        return Ok(ApiResponseFactory.Success(true));
    }
}