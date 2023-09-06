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
    public async Task<IActionResult> GetAllGroceryItems()
    {
        var groceryItems = await _service.GetAllGroceryItems();
        if (!groceryItems.Any())
            return NotFound();
        return Ok(groceryItems);
    }

    [HttpGet("Random/{quantity:int}")]
    [ProducesResponseType(typeof(List<GroceryItemModel>), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetRandomGroceryItems(int quantity)
    {
        if (quantity <= 0)
            return BadRequest();
        var randomGroceryItems = await _service.GetRandomGroceryItems(quantity);
        return Ok(randomGroceryItems);
    }

    [HttpPost(Name = "CreateGroceryItem")]
    [ProducesResponseType(typeof(GroceryItemModel), 201)]
    public async Task<IActionResult> CreateGroceryItem(GroceryItemModel groceryItem)
    {
        var result = await _service.CreateGroceryItem(groceryItem);
        return Created(nameof(GroceryItemModel), result);
    }

    [HttpGet("{groceryId:length(24)}", Name = "GetGroceryItemById")]
    [ProducesResponseType(typeof(GroceryItemModel), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetGroceryItemById(string groceryId)
    {
        var result = await _service.GetGroceryItemById(groceryId);
        if (result == null!)
            return NotFound();
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(GroceryItemModel), 200)]
    [ProducesResponseType(typeof(GroceryItemModel), 400)]
    public async Task<IActionResult> UpdateGroceryItem(GroceryItemModel groceryItem)
    {
        await _service.UpdateGroceryItem(groceryItem);
        return Accepted();
    }

    [HttpDelete]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> DeleteGroceryItem(string groceryId)
    {
        if (string.IsNullOrEmpty(groceryId))
            return BadRequest();
        await _service.DeleteGroceryItem(groceryId);
        return Accepted();
    }
}