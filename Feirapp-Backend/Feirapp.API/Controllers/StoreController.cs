using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.Stores.Dtos.Commands;
using Feirapp.Domain.Services.Stores.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("api/store")]
public class StoreController(IStoreService storeService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreatedResult), 201)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> CreateStore(InsertStoreCommand store, CancellationToken ct = default)
    {
        await storeService.InsertStoreAsync(store, ct);
        return Created();
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<StoreDto>), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetAllStores(CancellationToken ct = default)
    {
        var stores = await storeService.GetAllStoresAsync(ct);
        if (stores.Count == 0)
            return NotFound("Stores not found.");

        return Ok(stores);
    }

    [HttpGet("by-id")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(StoreDto), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetAllStores([FromQuery] long storeId, CancellationToken ct = default)
    {
        var store = await storeService.GetStoreById(storeId, ct);
        if (store == null)
            return NotFound("Stores not found.");

        return Ok(store);
    }
}