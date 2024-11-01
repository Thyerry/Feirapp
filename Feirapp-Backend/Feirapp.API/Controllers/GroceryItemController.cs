using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Domain.Services.GroceryItems.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
//[Authorize]
[Route("api/grocery-item")]
public class GroceryItemController(
    IGroceryItemService groceryItemService,
    IInvoiceReaderService invoiceService,
    ILogger<GroceryItemController> logger) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<List<SearchGroceryItemsResponse>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> SearchGroceryItems([FromQuery]SearchGroceryItemsQuery query, CancellationToken ct = default)
    {
        var groceryItems = await groceryItemService.SearchGroceryItemsAsync(query, ct);
        return groceryItems.Count == 0 
            ? BadRequest(ApiResponseFactory.Failure<List<SearchGroceryItemsResponse>>("No Grocery Items Found")) 
            : Ok(ApiResponseFactory.Success(groceryItems));
    }
    
    [HttpGet("by-id")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<GetGroceryItemByIdResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetById([FromQuery]long id, CancellationToken ct = default)
    {
        var result = await groceryItemService.GetByIdAsync(id, ct);
    
        return result == null 
            ? BadRequest(ApiResponseFactory.Failure<GetGroceryItemByIdResponse>("Grocery item not found")) 
            : Ok(ApiResponseFactory.Success(result));
    }

    [HttpGet("by-store")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<GetGroceryItemFromStoreIdResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetFromStore([FromQuery]long storeId, CancellationToken ct = default)
    {
        var groceryItems = await groceryItemService.GetByStoreAsync(storeId, ct);
        return groceryItems.Items.Count == 0 
            ? BadRequest(ApiResponseFactory.Failure<GetGroceryItemFromStoreIdResponse>("Store not found")) 
            : Ok(ApiResponseFactory.Success(groceryItems));
    }
    
    [HttpGet("by-invoice")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(InvoiceImportResponse), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetFromInvoice([FromQuery]string invoiceId, CancellationToken ct = default)
    {
        var groceryItems = await invoiceService.InvoiceDataScrapperAsync(invoiceId, false, ct);
        return groceryItems.Items.Count == 0 
            ? BadRequest(ApiResponseFactory.Failure<InvoiceImportResponse>("Grocery items not found"))
            : Ok(ApiResponseFactory.Success(groceryItems));
    }
    
    [HttpGet("random")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<List<SearchGroceryItemsResponse>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetRandomGroceryItems([FromQuery]int quantity, CancellationToken ct = default)
    {
        if (quantity <= 0)
            return BadRequest(ApiResponseFactory.Failure<List<SearchGroceryItemsResponse>>("Quantity must be greater than 0"));
        
        var randomGroceryItems = await groceryItemService.GetRandomGroceryItemsAsync(quantity, ct);
        return Ok(ApiResponseFactory.Success(randomGroceryItems));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<bool>), 201)]
    public async Task<IActionResult> Insert(InsertGroceryItemCommand command, CancellationToken ct = default)
    {
        await groceryItemService.InsertAsync(command, ct);
        return Created(nameof(command), ApiResponseFactory.Success(true));
    }
    
    [HttpPost("insert-list")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 201)]
    public async Task<IActionResult> InsertList(InsertListOfGroceryItemsCommand command, CancellationToken ct = default)
    {
        await groceryItemService.InsertListAsync(command, ct);
        return Created(nameof(command), ApiResponseFactory.Success(true));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<bool>), 204)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> Update([FromBody]UpdateGroceryItemCommand groceryItem, CancellationToken ct = default)
    {
        await groceryItemService.UpdateAsync(groceryItem, ct);
        return Accepted(ApiResponseFactory.Success(true));
    }

    [HttpDelete]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> Delete([FromQuery]long groceryId, CancellationToken ct = default)
    {
        await groceryItemService.DeleteAsync(groceryId, ct);
        return Accepted(ApiResponseFactory.Success(true));
    }
}