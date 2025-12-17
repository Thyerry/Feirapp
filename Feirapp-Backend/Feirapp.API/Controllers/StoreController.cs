using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;
using Feirapp.Domain.Services.Stores.Methods.SearchStores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Authorize]
[Route("api/store")]
public class StoreController(IStoreService storeService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateStore(InsertStoreRequest store, CancellationToken ct = default)
    {
        var result = await storeService.InsertStoreAsync(store, ct);
        if (!result.Success)
            return BadRequest(ApiResponseFactory.FromResult(result));

        return Created(nameof(store), ApiResponseFactory.FromResult(result));
    }

    [HttpGet]
    public async Task<IActionResult> SearchStores([FromQuery] SearchStoresRequest request, CancellationToken ct = default)
    {
        var result = await storeService.SearchStoresAsync(request, ct);
        if (!result.Success)
            return BadRequest(ApiResponseFactory.FromResult(result));

        var stores = result.Value ?? [];
        return stores.Count == 0 
            ? NotFound(ApiResponseFactory.Failure<List<SearchStoresResponse>>("No stores found")) 
            : Ok(ApiResponseFactory.Success(stores));
    }

    [HttpGet("by-id")]
    [AllowAnonymous]
    public async Task<IActionResult> GetStoreById([FromQuery] Guid storeId, CancellationToken ct = default)
    {
        var result = await storeService.GetStoreByIdAsync(storeId, ct);
        return !result.Success
            ? NotFound(ApiResponseFactory.FromResult(result))
            : Ok(ApiResponseFactory.FromResult(result));
    }
}