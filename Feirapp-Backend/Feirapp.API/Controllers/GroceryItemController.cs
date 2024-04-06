using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroceryItemController : ControllerBase
{
    private readonly IGroceryItemService _service;

    public GroceryItemController(IGroceryItemService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<GroceryItemDto>), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var groceryItems = await _service.GetAllAsync(ct);
        if (!groceryItems.Any())
            return NotFound();
        return Ok(groceryItems);
    }

    [HttpGet("Random/{quantity:int}")]
    [ProducesResponseType(typeof(List<GroceryItemDto>), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetRandomGroceryItems(int quantity, CancellationToken ct = default)
    {
        if (quantity <= 0)
            return BadRequest();
        var randomGroceryItems = await _service.GetRandomGroceryItemsAsync(quantity, ct);
        return Ok(randomGroceryItems);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GroceryItemDto), 201)]
    public async Task<IActionResult> Insert(GroceryItemDto groceryItem, CancellationToken ct = default)
    {
        var result = await _service.InsertAsync(groceryItem, ct);
        return Created(nameof(GroceryItemDto), result);
    }

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(GroceryItemDto), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetById(string id, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Invalid id");

        var result = await _service.GetById(id, ct);

        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(GroceryItemDto), 200)]
    [ProducesResponseType(typeof(GroceryItemDto), 400)]
    public async Task<IActionResult> Update(GroceryItemDto groceryItem, CancellationToken ct = default)
    {
        await _service.UpdateAsync(groceryItem, ct);
        return Accepted();
    }

    [HttpDelete]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> Delete(string groceryId, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(groceryId))
            return BadRequest();
        await _service.DeleteAsync(groceryId, ct);
        return Accepted();
    }
}