using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

/// <summary>
/// Controller for handling operations related to Import Grocery Items.
/// </summary>
[ApiController]
[Route("api/[controller]")]
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

    /// <summary>
    /// Gets the grocery items for a given invoice code.
    /// </summary>
    /// <param name="invoiceCode">The invoice code.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the action result.</returns>
    [HttpGet("invoice")]
    public async Task<IActionResult> ImportFromInvoice([FromQuery] string invoiceCode, CancellationToken ct)
    {
        var result = await _invoiceReaderService.InvoiceDataScrapperAsync(invoiceCode, true, ct);
        if (!result.Items.Any())
            return NotFound();

        return Ok();
    }

    [HttpPut("update-ncm")]
    public async Task<IActionResult> ImportFromNcmCest(CancellationToken ct)
    {
        await _ncmCestDataScrapper.UpdateNcmAndCestsDetails(ct);
        return Ok();
    }
}