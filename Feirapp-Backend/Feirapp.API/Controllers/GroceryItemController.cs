using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Domain.Services.GroceryItems.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Feirapp.API.Controllers;

/// <summary>
/// Controller for managing grocery item-related operations.
/// </summary>
[ApiController]
//[Authorize]
[Route("api/grocery-item")]
public class GroceryItemController(
    IGroceryItemService groceryItemService,
    IInvoiceReaderService invoiceService,
    ILogger<GroceryItemController> logger) : ControllerBase
{
    /// <summary>
    /// Searches for grocery items based on the provided query.
    /// </summary>
    /// <param name="query">The search query parameters.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP 200 OK status with a list of grocery items, or HTTP 404 Not Found if no items are found.</returns>
    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Searches for grocery items", Description = "Searches for grocery items based on the provided query.")]
    [SwaggerResponse(200, "List of grocery items retrieved successfully.", typeof(ApiResponse<List<SearchGroceryItemsResponse>>))]
    [SwaggerResponse(404, "No grocery items found.", typeof(ApiResponse<object>))]
    public async Task<IActionResult> SearchGroceryItems([FromQuery]SearchGroceryItemsQuery query, CancellationToken ct = default)
    {
        var groceryItems = await groceryItemService.SearchGroceryItemsAsync(query, ct);
        return groceryItems.Count == 0
            ? BadRequest(ApiResponseFactory.Failure<List<SearchGroceryItemsResponse>>("No Grocery Items Found"))
            : Ok(ApiResponseFactory.Success(groceryItems));
    }

    /// <summary>
    /// Retrieves a grocery item by its ID.
    /// </summary>
    /// <param name="id">The ID of the grocery item to retrieve.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP 200 OK status with the grocery item data, or HTTP 404 Not Found if the item is not found.</returns>
    [HttpGet("by-id")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Retrieves a grocery item by ID", Description = "Retrieves a grocery item by its ID.")]
    [SwaggerResponse(200, "Grocery item retrieved successfully.", typeof(ApiResponse<GetGroceryItemByIdResponse>))]
    [SwaggerResponse(404, "Grocery item not found.", typeof(ApiResponse<object>))]
    public async Task<IActionResult> GetById([FromQuery]long id, CancellationToken ct = default)
    {
        var result = await groceryItemService.GetByIdAsync(id, ct);

        return result == null
            ? BadRequest(ApiResponseFactory.Failure<GetGroceryItemByIdResponse>("Grocery item not found"))
            : Ok(ApiResponseFactory.Success(result));
    }

    /// <summary>
    /// Retrieves grocery items from a specific store by the store's ID.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP 200 OK status with the grocery items data, or HTTP 404 Not Found if the store is not found.</returns>
    [HttpGet("by-store")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Retrieves grocery items from a store", Description = "Retrieves grocery items from a specific store by the store's ID.")]
    [SwaggerResponse(200, "Grocery items retrieved successfully.", typeof(ApiResponse<GetGroceryItemFromStoreIdResponse>))]
    [SwaggerResponse(404, "Store not found.", typeof(ApiResponse<object>))]
    public async Task<IActionResult> GetFromStore([FromQuery]long storeId, CancellationToken ct = default)
    {
        var groceryItems = await groceryItemService.GetByStoreAsync(storeId, ct);
        return groceryItems.Items.Count == 0
            ? BadRequest(ApiResponseFactory.Failure<GetGroceryItemFromStoreIdResponse>("Store not found"))
            : Ok(ApiResponseFactory.Success(groceryItems));
    }

    /// <summary>
    /// Retrieves grocery items from an invoice by the invoice ID.
    /// </summary>
    /// <param name="invoiceId">The ID of the invoice.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP 200 OK status with the grocery items data, or HTTP 404 Not Found if no items are found.</returns>
    [HttpGet("by-invoice")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Retrieves grocery items from an invoice", Description = "Retrieves grocery items from an invoice by the invoice ID.")]
    [SwaggerResponse(200, "Grocery items retrieved successfully.", typeof(InvoiceImportResponse))]
    [SwaggerResponse(404, "Grocery items not found.", typeof(ApiResponse<object>))]
    public async Task<IActionResult> GetFromInvoice([FromQuery]string invoiceId, CancellationToken ct = default)
    {
        var groceryItems = await invoiceService.InvoiceDataScrapperAsync(invoiceId, false, ct);
        return groceryItems.Items.Count == 0
            ? BadRequest(ApiResponseFactory.Failure<InvoiceImportResponse>("Grocery items not found"))
            : Ok(ApiResponseFactory.Success(groceryItems));
    }

    /// <summary>
    /// Retrieves a random set of grocery items.
    /// </summary>
    /// <param name="quantity">The number of random grocery items to retrieve.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP 200 OK status with a list of random grocery items, or HTTP 400 Bad Request if the quantity is invalid.</returns>
    [HttpGet("random")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Retrieves random grocery items", Description = "Retrieves a random set of grocery items.")]
    [SwaggerResponse(200, "Random grocery items retrieved successfully.", typeof(ApiResponse<List<SearchGroceryItemsResponse>>))]
    [SwaggerResponse(400, "Invalid quantity.", typeof(ApiResponse<object>))]
    public async Task<IActionResult> GetRandomGroceryItems([FromQuery]int quantity, CancellationToken ct = default)
    {
        if (quantity <= 0)
            return BadRequest(ApiResponseFactory.Failure<List<SearchGroceryItemsResponse>>("Quantity must be greater than 0"));

        var randomGroceryItems = await groceryItemService.GetRandomGroceryItemsAsync(quantity, ct);
        return Ok(ApiResponseFactory.Success(randomGroceryItems));
    }

    /// <summary>
    /// Inserts a new grocery item.
    /// </summary>
    /// <param name="command">The command containing the grocery item data to insert.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP 201 Created status.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Inserts a new grocery item", Description = "Inserts a new grocery item.")]
    [SwaggerResponse(201, "Grocery item created successfully.", typeof(ApiResponse<bool>))]
    public async Task<IActionResult> Insert(InsertGroceryItemCommand command, CancellationToken ct = default)
    {
        await groceryItemService.InsertAsync(command, ct);
        return Created(nameof(command), ApiResponseFactory.Success(true));
    }

    /// <summary>
    /// Inserts a list of new grocery items.
    /// </summary>
    /// <param name="command">The command containing the list of grocery items to insert.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP 201 Created status.</returns>
    [HttpPost("insert-list")]
    [SwaggerOperation(Summary = "Inserts a list of new grocery items", Description = "Inserts a list of new grocery items.")]
    [SwaggerResponse(201, "List of grocery items created successfully.", typeof(ApiResponse<bool>))]
    public async Task<IActionResult> InsertList(InsertListOfGroceryItemsCommand command, CancellationToken ct = default)
    {
        await groceryItemService.InsertListAsync(command, ct);
        return Created(nameof(command), ApiResponseFactory.Success(true));
    }

    /// <summary>
    /// Updates an existing grocery item.
    /// </summary>
    /// <param name="groceryItem">The grocery item data to update.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP 204 No Content status, or HTTP 400 Bad Request if the update fails.</returns>
    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing grocery item", Description = "Updates an existing grocery item.")]
    [SwaggerResponse(204, "Grocery item updated successfully.", typeof(ApiResponse<bool>))]
    [SwaggerResponse(400, "Invalid grocery item data.", typeof(ApiResponse<object>))]
    public async Task<IActionResult> Update([FromBody]UpdateGroceryItemCommand groceryItem, CancellationToken ct = default)
    {
        await groceryItemService.UpdateAsync(groceryItem, ct);
        return Accepted(ApiResponseFactory.Success(true));
    }

    /// <summary>
    /// Deletes a grocery item by its ID.
    /// </summary>
    /// <param name="groceryId">The ID of the grocery item to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP 200 OK status, or HTTP 400 Bad Request if the deletion fails.</returns>
    [HttpDelete]
    [SwaggerOperation(Summary = "Deletes a grocery item", Description = "Deletes a grocery item by its ID.")]
    [SwaggerResponse(200, "Grocery item deleted successfully.", typeof(OkResult))]
    [SwaggerResponse(400, "Invalid grocery item ID.", typeof(ApiResponse<object>))]
    public async Task<IActionResult> Delete([FromQuery][SwaggerParameter("blablabals")]long groceryId, CancellationToken ct = default)
    {
        await groceryItemService.DeleteAsync(groceryId, ct);
        return Accepted(ApiResponseFactory.Success(true));
    }
}