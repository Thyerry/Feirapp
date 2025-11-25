using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("api/import")]
public class ImportController(INcmCestDataScrapper ncmCestDataScrapper) : ControllerBase
{
    [HttpPut("update-ncm")]
    public async Task<IActionResult> UpdateNcmAndCestsDetails(CancellationToken ct)
    {
        await ncmCestDataScrapper.UpdateNcmAndCestsDetailsAsync(ct);
        return Ok(ApiResponseFactory.Success(true));
    }
}