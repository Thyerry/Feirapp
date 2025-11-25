using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Domain.Services.Stores.Methods.GetStoreById;
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
        await storeService.InsertStoreAsync(store, ct);
        return Created();
    }

    [HttpGet]
    public async Task<IActionResult> SearchStores([FromQuery] SearchStoresRequest request, CancellationToken ct = default)
    {
        var stores = await storeService.SearchStoresAsync(request, ct);
        return stores.Count == 0 
            ? NotFound(ApiResponseFactory.Failure<List<SearchStoresResponse>>("No stores found")) 
            : Ok(ApiResponseFactory.Success(stores));
    }

    [HttpGet("by-id")]
    [AllowAnonymous]
    public async Task<IActionResult> GetStoreById([FromQuery] Guid storeId, CancellationToken ct = default)
    {
        var store = await storeService.GetStoreByIdAsync(storeId, ct);
        return store == null
            ? NotFound(ApiResponseFactory.Failure<GetStoreByIdResponse>("Stores not found."))
            : Ok(ApiResponseFactory.Success(store));
    }
}