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
    
    [HttpGet("Name/{groceryItemName}", Name = "GetGroceryItemsByName")]
    [ProducesResponseType(typeof(List<GroceryItem>), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetGroceryItemsByName([FromRoute]string groceryItemName)
    {
        var groceryItems = await _service.GetGroceryItemByName(groceryItemName);
        if(!groceryItems.Any())
            return NotFound();
        return Ok(groceryItems);
     }

    [HttpPost(Name = "CreateGroceryItem")]
    [ProducesResponseType(typeof(GroceryItem), 201)]
    public async Task<IActionResult> CreateGroceryItem([FromBody]GroceryItem groceryItem)
    {
        try
        {
            var result = await _service.CreateGroceryItem(groceryItem);
            return Created(nameof(GroceryItem), result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{groceryId}", Name = "GetGroceryItemById")]
    [ProducesResponseType(typeof(GroceryItem), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetGroceryItemById([FromRoute]string groceryId)
    {
        var result = await _service.GetGroceryItemById(groceryId);
        if(result == null!)
            return NotFound();
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(GroceryItem), 200)]
    [ProducesResponseType(typeof(GroceryItem), 400)]
    public async Task<IActionResult> UpdateGroceryItem(GroceryItem groceryItem)
    {
        try
        {
            var result = await _service.UpdateGroceryItem(groceryItem);
            return Ok(result);
        }
        catch (Exception)
        {
            return BadRequest();
        }
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