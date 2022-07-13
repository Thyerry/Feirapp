using Feirapp.Domain.Contracts;
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
    public async Task<IActionResult> GetAllGroceryItems()
    {
        var groceryItems = await _service.GetAllGroceryItems();
        if (!groceryItems.Any())
            return NotFound();
        return Ok(groceryItems);
    }
    
    [HttpGet]
    [Route("{name}")]
    public async Task<IActionResult> GetByName([FromRoute]string name)
    {
        var groceryItems = await _service.GetByName(name);
        if(!groceryItems.Any())
            return NotFound();
        return Ok(groceryItems);
    }
}