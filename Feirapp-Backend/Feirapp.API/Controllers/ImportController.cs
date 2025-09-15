using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("api/import")]
public class ImportController : ControllerBase
{
    private readonly IInvoiceReaderService _invoiceReaderService;
    private readonly INcmCestDataScrapper _ncmCestDataScrapper;

    public ImportController(IInvoiceReaderService invoiceReaderService, INcmCestDataScrapper ncmCestDataScrapper)
    {
        _invoiceReaderService = invoiceReaderService;
        _ncmCestDataScrapper = ncmCestDataScrapper;
    }

    [HttpPut("update-ncm")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    public async Task<IActionResult> UpdateNcmAndCestsDetails(CancellationToken ct)
    {
        await _ncmCestDataScrapper.UpdateNcmAndCestsDetailsAsync(ct);
        return Ok(ApiResponseFactory.Success(true));
    }
}