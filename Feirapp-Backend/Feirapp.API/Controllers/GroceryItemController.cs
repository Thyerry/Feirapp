using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GroceryItemController : ControllerBase
{
    private readonly IGroceryItemService _service;

    public GroceryItemController(IGroceryItemService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<GroceryItemModel>), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetAll(CancellationToken cancellation = default)
    {
        var groceryItems = await _service.GetAllGroceryItems();
        if (!groceryItems.Any())
            return NotFound();
        return Ok(groceryItems);
    }

    [HttpGet("Random/{quantity:int}")]
    [ProducesResponseType(typeof(List<GroceryItemModel>), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetRandomGroceryItems(int quantity, CancellationToken cancellationToken = default)
    {
        if (quantity <= 0)
            return BadRequest();
        var randomGroceryItems = await _service.GetRandomGroceryItems(quantity);
        return Ok(randomGroceryItems);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GroceryItemModel), 201)]
    public async Task<IActionResult> Insert(GroceryItemModel groceryItem, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateGroceryItem(groceryItem);
        return Created(nameof(GroceryItemModel), result);
    }

    [HttpGet("{groceryId:length(24)}")]
    [ProducesResponseType(typeof(GroceryItemModel), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetById(string groceryId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetGroceryItemById(groceryId);
        if (result == null!)
            return NotFound();
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(GroceryItemModel), 200)]
    [ProducesResponseType(typeof(GroceryItemModel), 400)]
    public async Task<IActionResult> Update(GroceryItemModel groceryItem, CancellationToken cancellationToken = default)
    {
        await _service.UpdateGroceryItem(groceryItem);
        return Accepted();
    }

    [HttpDelete]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> Delete(string groceryId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(groceryId))
            return BadRequest();
        await _service.DeleteGroceryItem(groceryId);
        return Accepted();
    }
}