using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.Stores.Dtos.Commands;
using Feirapp.Domain.Services.Stores.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("api/store")]
public class StoreController(IStoreService storeService) : ControllerBase
{
    private readonly IStoreService _storeService = storeService;
    
    
    [HttpPost]
    [ProducesResponseType(typeof(CreatedResult), 201)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> CreateStore(InsertStoreCommand store, CancellationToken ct = default)
    {
        await _storeService.InsertStoreAsync(store, ct);
        return Created();
    }
    [HttpGet]
    [ProducesResponseType(typeof(List<StoreDto>), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetAllStores(CancellationToken ct = default)
    {
        var stores = await _storeService.GetAllStoresAsync(ct);
        if (stores.Count == 0)
            return NotFound("No Stores Found");

        return Ok(stores);
    }
    
    [HttpGet("by-id")]
    [ProducesResponseType(typeof(StoreDto), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetAllStores([FromQuery] long storeId, CancellationToken ct = default)
    {
        var store = await _storeService.GetStoreById(storeId, ct);
        if (store == null)  
            return NotFound("No Stores Found");

        return Ok(store);
    }
}