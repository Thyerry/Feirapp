using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.Stores.Dtos.Commands;
using Feirapp.Domain.Services.Stores.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Feirapp.API.Controllers;

/// <summary>
/// Controller for managing store-related operations.
/// </summary>
[ApiController]
[Route("api/store")]
public class StoreController(IStoreService storeService) : ControllerBase
{
    /// <summary>
    /// Creates a new store.
    /// </summary>
    /// <param name="store">The store data to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP 201 Created status.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new store", Description = "Creates a new store with the provided data.")]
    [SwaggerResponse(201, "Store created successfully.")]
    public async Task<IActionResult> CreateStore(InsertStoreCommand store, CancellationToken ct = default)
    {
        await storeService.InsertStoreAsync(store, ct);
        return Created();
    }

    /// <summary>
    /// Retrieves all stores.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP 200 OK status with a list of stores, or HTTP 404 Not Found if no stores are found.</returns>
    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Retrieves all stores", Description = "Retrieves a list of all stores.")]
    [SwaggerResponse(200, "List of stores retrieved successfully.", typeof(ApiResponse<List<StoreDto>>))]
    [SwaggerResponse(404, "No stores found.", typeof(ApiResponse<object>))]
    public async Task<IActionResult> GetAllStores(CancellationToken ct = default)
    {
        var stores = await storeService.GetAllStoresAsync(ct);

        return stores.Count == 0
            ? NotFound(ApiResponseFactory.Failure<List<StoreDto>>("Stores not found."))
            : Ok(ApiResponseFactory.Success(stores));
    }

    /// <summary>
    /// Retrieves a store by its ID.
    /// </summary>
    /// <param name="storeId">The ID of the store to retrieve.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP 200 OK status with the store data, or HTTP 404 Not Found if the store is not found.</returns>
    [HttpGet("by-id")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Retrieves a store by ID", Description = "Retrieves a store by its ID.")]
    [SwaggerResponse(200, "Store retrieved successfully.", typeof(ApiResponse<StoreDto>))]
    [SwaggerResponse(400, "Invalid store id.", typeof(ApiResponse<StoreDto>))]
    [SwaggerResponse(404, "Store not found.", typeof(ApiResponse<object>))]
    public async Task<IActionResult> GetAllStores([FromQuery] long storeId, CancellationToken ct = default)
    {
        if (storeId <= 0)
            return BadRequest(ApiResponseFactory.Failure<StoreDto>("Invalid store id."));

        var store = await storeService.GetStoreById(storeId, ct);
        return store == null
            ? NotFound(ApiResponseFactory.Failure<StoreDto>("Stores not found."))
            : Ok(ApiResponseFactory.Success(store));
    }
}