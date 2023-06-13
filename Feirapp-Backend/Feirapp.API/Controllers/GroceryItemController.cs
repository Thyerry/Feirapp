using Feirapp.Domain.Contracts;
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
    [ProducesResponseType(typeof(List<GroceryItem>), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetAllGroceryItems()
    {
        var groceryItems = await _service.GetAllGroceryItems();
        if (!groceryItems.Any())
            return NotFound();
        return Ok(groceryItems);
    }

    [HttpGet("Random/{quantity:int}")]
    [ProducesResponseType(typeof(List<GroceryItem>), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetRandomGroceryItems(int quantity)
    {
        if (quantity <= 0)
            return BadRequest();
        var randomGroceryItems = await _service.GetRandomGroceryItems(quantity);
        return Ok(randomGroceryItems);
    }

    [HttpGet("Name/{groceryItemName}", Name = "GetGroceryItemsByName")]
    [ProducesResponseType(typeof(List<GroceryItem>), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetGroceryItemsByName([FromRoute] string groceryItemName)
    {
        var groceryItems = await _service.GetGroceryItemByName(groceryItemName);
        if (!groceryItems.Any())
            return NotFound();
        return Ok(groceryItems);
    }

    [HttpPost(Name = "CreateGroceryItem")]
    [ProducesResponseType(typeof(GroceryItem), 201)]
    public async Task<IActionResult> CreateGroceryItem(GroceryItem groceryItem)
    {
        var result = await _service.CreateGroceryItem(groceryItem);
        return Created(nameof(GroceryItem), result);
    }

    [HttpGet("{groceryId:length(24)}", Name = "GetGroceryItemById")]
    [ProducesResponseType(typeof(GroceryItem), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetGroceryItemById(string groceryId)
    {
        var result = await _service.GetGroceryItemById(groceryId);
        if (result == null!)
            return NotFound();
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(GroceryItem), 200)]
    [ProducesResponseType(typeof(GroceryItem), 400)]
    public async Task<IActionResult> UpdateGroceryItem(GroceryItem groceryItem)
    {
        var result = await _service.UpdateGroceryItem(groceryItem);
        return Ok(result);
    }

    [HttpDelete]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> DeleteGroceryItem(string groceryId)
    {
        if (string.IsNullOrEmpty(groceryId))
            return BadRequest();
        await _service.DeleteGroceryItem(groceryId);
        return Ok();
    }
}