using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.Stores.Dtos.Commands;
using Feirapp.Domain.Services.Stores.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("api/store")]
public class StoreController(IStoreService storeService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateStore(InsertStoreCommand store, CancellationToken ct = default)
    {
        await storeService.InsertStoreAsync(store, ct);
        return Created();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllStores(CancellationToken ct = default)
    {
        var stores = await storeService.GetAllStoresAsync(ct);

        return stores.Count == 0
            ? NotFound(ApiResponseFactory.Failure<List<StoreDto>>("Stores not found."))
            : Ok(ApiResponseFactory.Success(stores));
    }

    [HttpGet("by-id")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllStores([FromQuery] Guid storeId, CancellationToken ct = default)
    {
        var store = await storeService.GetStoreById(storeId, ct);
        return store == null
            ? NotFound(ApiResponseFactory.Failure<StoreDto>("Stores not found."))
            : Ok(ApiResponseFactory.Success(store));
    }
}