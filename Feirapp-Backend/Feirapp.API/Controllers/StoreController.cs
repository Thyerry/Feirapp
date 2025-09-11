using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.GroceryItems.Misc;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Domain.Services.Stores.Methods.GetStoreById;
using Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("api/store")]
public class StoreController(IStoreService storeService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateStore(InsertStoreRequest store, CancellationToken ct = default)
    {
        await storeService.InsertStoreAsync(store, ct);
        return Created();
    }

    [HttpGet("by-id")]
    [AllowAnonymous]
    public async Task<IActionResult> GetStoreById([FromQuery] Guid storeId, CancellationToken ct = default)
    {
        var store = await storeService.GetStoreById(storeId, ct);
        return store == null
            ? NotFound(ApiResponseFactory.Failure<GetStoreByIdResponse>("Stores not found."))
            : Ok(ApiResponseFactory.Success(store));
    }
}