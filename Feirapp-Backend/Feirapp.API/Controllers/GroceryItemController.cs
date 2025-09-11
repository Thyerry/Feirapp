using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItem;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertListOfGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.UpdateGroceryItemCommand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
//[Authorize]
[Route("api/grocery-item")]
public class GroceryItemController(IGroceryItemService groceryItemService, IInvoiceReaderService invoiceService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> SearchGroceryItems([FromQuery]SearchGroceryItemsRequest request, CancellationToken ct = default)
    {
        var groceryItems = await groceryItemService.SearchGroceryItemsAsync(request, ct);
        return groceryItems.Count == 0
            ? NotFound(ApiResponseFactory.Failure<List<SearchGroceryItemsResponse>>("No Grocery Items Found"))
            : Ok(ApiResponseFactory.Success(groceryItems));
    }

    [HttpGet("by-id")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromQuery]Guid id, CancellationToken ct = default)
    {
        var result = await groceryItemService.GetByIdAsync(id, ct);

        return result == null
            ? NotFound(ApiResponseFactory.Failure<GetGroceryItemByIdResponse>("Grocery item not found"))
            : Ok(ApiResponseFactory.Success(result));
    }

    [HttpGet("by-store")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFromStore([FromQuery]Guid storeId, CancellationToken ct = default)
    {
        var groceryItems = await groceryItemService.GetByStoreAsync(storeId, ct);
        return groceryItems.Items.Count == 0
            ? BadRequest(ApiResponseFactory.Failure<GetGroceryItemFromStoreIdResponse>("Store not found"))
            : Ok(ApiResponseFactory.Success(groceryItems));
    }

    [HttpGet("by-invoice")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFromInvoice([FromQuery]string invoiceId, CancellationToken ct = default)
    {
        var groceryItems = await invoiceService.InvoiceDataScrapperAsync(invoiceId, false, ct);
        return groceryItems.Items.Count == 0
            ? BadRequest(ApiResponseFactory.Failure<InvoiceImportResponse>("Grocery items not found"))
            : Ok(ApiResponseFactory.Success(groceryItems));
    }

    [HttpGet("random")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRandomGroceryItems([FromQuery]int quantity, CancellationToken ct = default)
    {
        if (quantity <= 0)
            return BadRequest(ApiResponseFactory.Failure<List<SearchGroceryItemsResponse>>("Quantity must be greater than 0"));

        var randomGroceryItems = await groceryItemService.GetRandomGroceryItemsAsync(quantity, ct);
        return Ok(ApiResponseFactory.Success(randomGroceryItems));
    }

    [HttpPost]
    public async Task<IActionResult> Insert(InsertGroceryItemRequest request, CancellationToken ct = default)
    {
        await groceryItemService.InsertAsync(request, ct);
        return Created(nameof(request), ApiResponseFactory.Success(true));
    }

    [HttpPost("insert-list")]
    public async Task<IActionResult> InsertList(InsertListOfGroceryItemsRequest request, CancellationToken ct = default)
    {
        await groceryItemService.InsertListAsync(request, ct);
        return Created(nameof(request), ApiResponseFactory.Success(true));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]UpdateGroceryItemCommand groceryItem, CancellationToken ct = default)
    {
        await groceryItemService.UpdateAsync(groceryItem, ct);
        return Accepted(ApiResponseFactory.Success(true));
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery]Guid groceryId, CancellationToken ct = default)
    {
        await groceryItemService.DeleteAsync(groceryId, ct);
        return Accepted(ApiResponseFactory.Success(true));
    }
}