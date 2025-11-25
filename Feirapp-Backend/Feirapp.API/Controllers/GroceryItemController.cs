using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.DataScrapper.Methods.InvoiceScan;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
// [Authorize]
[Route("api/grocery-item")]
public class GroceryItemController(IGroceryItemService groceryItemService, IInvoiceReaderService invoiceService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> SearchGroceryItems([FromQuery] SearchGroceryItemsRequest request, CancellationToken ct = default)
    {
        var groceryItems = await groceryItemService.SearchAsync(request, ct);
        return groceryItems.Count == 0
            ? NotFound(ApiResponseFactory.Failure<List<SearchGroceryItemsResponse>>("No Grocery Items Found"))
            : Ok(ApiResponseFactory.Success(groceryItems));
    }

    [HttpGet("by-id")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromQuery] Guid id, CancellationToken ct = default)
    {
        var result = await groceryItemService.GetByIdAsync(id, ct);

        return result == null
            ? NotFound(ApiResponseFactory.Failure<GetGroceryItemByIdResponse>("Grocery item not found"))
            : Ok(ApiResponseFactory.Success(result));
    }

    [HttpGet("by-store")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFromStore([FromQuery] Guid storeId, CancellationToken ct = default)
    {
        var groceryItems = await groceryItemService.GetByStoreAsync(storeId, ct);
        return groceryItems.Items.Count == 0
            ? NotFound(ApiResponseFactory.Failure<GetGroceryItemsByStoreIdResponse>("Store not found"))
            : Ok(ApiResponseFactory.Success(groceryItems));
    }

    [HttpGet("by-invoice")]
    public async Task<IActionResult> GetFromInvoice([FromQuery] string invoiceId, CancellationToken ct = default)
    {
        var groceryItems = await invoiceService.InvoiceImportAsync(invoiceId, false, ct);
        return groceryItems.Items.Count == 0
            ? NotFound(ApiResponseFactory.Failure<InvoiceImportResponse>("Grocery items not found"))
            : Ok(ApiResponseFactory.Success(groceryItems));
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] InsertGroceryItemsRequest request, CancellationToken ct = default)
    {
        await groceryItemService.InsertAsync(request, ct);
        return Created(nameof(request), ApiResponseFactory.Success(true));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] Guid id, CancellationToken ct = default)
    {
        await groceryItemService.DeleteAsync(id, ct);
        return Accepted(ApiResponseFactory.Success(true));
    }
}