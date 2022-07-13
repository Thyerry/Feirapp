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
        _service = service;
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
    
    [HttpGet("{groceryItemName}", Name = "GetGroceryItemsByName")]
    [ProducesResponseType(typeof(List<GroceryItem>), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetGroceryItemsByName([FromRoute]string groceryItemName)
    {
        var groceryItems = await _service.GetByName(groceryItemName);
        if(!groceryItems.Any())
            return NotFound();
        return Ok(groceryItems);
     }

    [HttpPost(Name = "CreateGroceryItem")]
    [ProducesResponseType(typeof(GroceryItem), 201)]
    public async Task<IActionResult> CreateGroceryItem([FromBody]GroceryItem groceryItem)
    {
        var result = await _service.CreateGroceryItem(groceryItem);
        return Created(nameof(GroceryItem), result);
    }
}