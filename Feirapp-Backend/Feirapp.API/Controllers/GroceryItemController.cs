using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroceryItemController(IGroceryItemService service, IInvoiceReaderService invoiceService, ILogger<GroceryItemController> logger) : ControllerBase
{
    private readonly IGroceryItemService _groceryItemService = service ?? throw new ArgumentNullException(nameof(service));
    private readonly IInvoiceReaderService _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
    private readonly ILogger<GroceryItemController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [HttpGet]
    [ProducesResponseType(typeof(GetGroceryItemResponse), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var groceryItems = await _groceryItemService.GetAllAsync(ct);
        if (groceryItems.Items.Count == 0)
            return NotFound("No Grocery Items Found");
 
        return Ok(groceryItems);
    }
    
    [HttpGet("getFromStore/{storeId}")]
    [ProducesResponseType(typeof(GetGroceryItemResponse), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetFromStore(long storeId, CancellationToken ct = default)
    {
        var groceryItems = await _groceryItemService.GetFromStoreAsync(storeId, ct);
        if (groceryItems.Items.Count == 0)
            return NotFound("No Grocery Items Found");

        return Ok(groceryItems);
    }
    
    [HttpGet("getFromInvoice/{invoiceId}")]
    [ProducesResponseType(typeof(GetGroceryItemResponse), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetFromInvoice(string invoiceId, CancellationToken ct = default)
    {
        var groceryItems = await _invoiceService.InvoiceDataScrapperAsync(invoiceId, false,ct);
        if (groceryItems.Items.Count == 0)
            return NotFound("No Grocery Items Found");

        return Ok(groceryItems);
    }

    // [HttpGet("Random/{quantity:int}")]
    // [ProducesResponseType(typeof(List<GroceryItemDto>), 200)]
    // [ProducesResponseType(typeof(BadRequest), 400)]
    // [ProducesResponseType(typeof(NotFoundResult), 404)]
    // public async Task<IActionResult> GetRandomGroceryItems(int quantity, CancellationToken ct = default)
    // {
    //     if (quantity <= 0)
    //         return BadRequest();
    //     var randomGroceryItems = await _service.GetRandomGroceryItemsAsync(quantity, ct);
    //     return Ok(randomGroceryItems);
    // }

    //[HttpPost]
    //[ProducesResponseType(typeof(GroceryItemDto), 201)]
    //public async Task<IActionResult> Insert(GroceryItemDto groceryItem, CancellationToken ct = default)
    //{
    //    var result = await _service.InsertAsync(groceryItem, ct);
    //    return Created(nameof(GroceryItemDto), result);
    //}

    //[HttpGet("{id}")]
    //[ProducesResponseType(typeof(GroceryItemDto), 200)]
    //[ProducesResponseType(typeof(NotFoundResult), 404)]
    //public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    //{
    //    var result = await _service.GetById(id, ct);

    //    if (result == null)
    //        return NotFound();
    //    return Ok(result);
    //}

    //[HttpPut]
    //[ProducesResponseType(typeof(GroceryItemDto), 200)]
    //[ProducesResponseType(typeof(GroceryItemDto), 400)]
    //public async Task<IActionResult> Update(GroceryItemDto groceryItem, CancellationToken ct = default)
    //{
    //    await _service.UpdateAsync(groceryItem, ct);
    //    return Accepted();
    //}

    //[HttpDelete("{id}")]
    //[ProducesResponseType(typeof(OkResult), 200)]
    //[ProducesResponseType(typeof(BadRequestResult), 400)]
    //public async Task<IActionResult> Delete(long groceryId, CancellationToken ct = default)
    //{
    //    await _service.DeleteAsync(groceryId, ct);
    //    return Accepted();
    //}
}