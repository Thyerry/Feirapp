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
        if(groceryItems.Any())
            return Ok(groceryItems);
        return NotFound();
    }
}