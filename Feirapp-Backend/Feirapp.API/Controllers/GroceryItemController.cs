using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Dtos;
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
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var groceryItems = await _service.GetAllAsync(cancellationToken);
        if (!groceryItems.Any())
            return NotFound();
        return Ok(groceryItems);
    }

    [HttpGet("Random/{quantity:int}")]
    [ProducesResponseType(typeof(List<GroceryItemDto>), 200)]
    [ProducesResponseType(typeof(BadRequest), 400)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetRandomGroceryItems(int quantity, CancellationToken cancellationToken = default)
    {
        if (quantity <= 0)
            return BadRequest();
        var randomGroceryItems = await _service.GetRandomGroceryItemsAsync(quantity, cancellationToken);
        return Ok(randomGroceryItems);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GroceryItemDto), 201)]
    public async Task<IActionResult> Insert(GroceryItemDto groceryItem, CancellationToken cancellationToken = default)
    {
        var result = await _service.InsertAsync(groceryItem, cancellationToken);
        return Created(nameof(GroceryItemDto), result);
    }

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(GroceryItemDto), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Invalid id");

        var result = await _service.GetById(id, cancellationToken);

        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(GroceryItemDto), 200)]
    [ProducesResponseType(typeof(GroceryItemDto), 400)]
    public async Task<IActionResult> Update(GroceryItemDto groceryItem, CancellationToken cancellationToken = default)
    {
        await _service.UpdateAsync(groceryItem, cancellationToken);
        return Accepted();
    }

    [HttpDelete]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> Delete(string groceryId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(groceryId))
            return BadRequest();
        await _service.DeleteAsync(groceryId, cancellationToken);
        return Accepted();
    }
}