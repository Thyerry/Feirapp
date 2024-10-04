using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Domain.Services.GroceryItems.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("api/grocery-item")]
public class GroceryItemController(
    IGroceryItemService groceryItemService,
    IInvoiceReaderService invoiceService,
    ILogger<GroceryItemController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<SearchGroceryItemsResponse>), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> SearchGroceryItems([FromQuery]SearchGroceryItemsQuery query, CancellationToken ct = default)
    {
        var groceryItems = await groceryItemService.SearchGroceryItemsAsync(query, ct);
        if (groceryItems.Count == 0)
            return NotFound("No Grocery Items Found");

        return Ok(groceryItems);
    }
    
    [HttpGet("by-id")]
    [ProducesResponseType(typeof(GroceryItemDto), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetById([FromQuery]long id, CancellationToken ct = default)
    {
        var result = await groceryItemService.GetByIdAsync(id, ct);
    
        if (result == null)
            return NotFound("Grocery item not found");
    
        return Ok(result);
    }

    [HttpGet("by-store")]
    [ProducesResponseType(typeof(GetGroceryItemFromStoreIdResponse), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetFromStore([FromQuery]long storeId, CancellationToken ct = default)
    {
        var groceryItems = await groceryItemService.GetByStoreAsync(storeId, ct);
        if (groceryItems.Items.Count == 0)
            return NotFound("Store not found");
    
        return Ok(groceryItems);
    }
    
    
    [HttpGet("by-invoice")]
    [ProducesResponseType(typeof(InvoiceImportResponse), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetFromInvoice([FromQuery]string invoiceId, CancellationToken ct = default)
    {
        var groceryItems = await invoiceService.InvoiceDataScrapperAsync(invoiceId, false, ct);
        if (groceryItems.Items.Count == 0)
            return NotFound("Grocery items not found");
    
        return Ok(groceryItems);
    }
    
    [HttpGet("random")]
    [ProducesResponseType(typeof(List<SearchGroceryItemsResponse>), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetRandomGroceryItems([FromQuery]int quantity, CancellationToken ct = default)
    {
        if (quantity <= 0)
            return BadRequest();
        var randomGroceryItems = await groceryItemService.GetRandomGroceryItemsAsync(quantity, ct);
        return Ok(randomGroceryItems);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(GroceryItemDto), 201)]
    public async Task<IActionResult> Insert(InsertGroceryItemCommand command, CancellationToken ct = default)
    {
        await groceryItemService.InsertAsync(command, ct);
        return Created();
    }
    
    [HttpPost("insert-list")]
    [ProducesResponseType(typeof(CreatedResult), 201)]
    public async Task<IActionResult> InsertList(InsertListOfGroceryItemsCommand command, CancellationToken ct = default)
    {
        await groceryItemService.InsertListAsync(command, ct);
        return Created();
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(AcceptedResult), 204)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> Update([FromBody]UpdateGroceryItemCommand groceryItem, CancellationToken ct = default)
    {
        await groceryItemService.UpdateAsync(groceryItem, ct);
        return Accepted();
    }

    [HttpDelete]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> Delete([FromQuery]long groceryId, CancellationToken ct = default)
    {
        await groceryItemService.DeleteAsync(groceryId, ct);
        return Accepted();
    }
}