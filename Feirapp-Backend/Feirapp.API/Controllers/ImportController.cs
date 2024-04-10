using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

/// <summary>
/// Controller for handling operations related to Import Grocery Items.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ImportController : ControllerBase
{
    private readonly IInvoiceReaderService _invoiceReaderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportController"/> class.
    /// </summary>
    /// <param name="invoiceReaderService">The service to read invoice data.</param>
    public ImportController(IInvoiceReaderService invoiceReaderService)
    {
        _invoiceReaderService = invoiceReaderService;
    }

    /// <summary>
    /// Gets the grocery items for a given invoice code.
    /// </summary>
    /// <param name="invoiceCode">The invoice code.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the action result.</returns>
    [HttpGet("invoice")]
    public async Task<IActionResult> ImportFromInvoice([FromQuery]string invoiceCode, CancellationToken ct)
    {
        var result = await _invoiceReaderService.InvoiceDataScrapperAsync(invoiceCode, ct);
        if (!result.Items.Any())
            return NotFound();

        return Ok(result);
    }
}