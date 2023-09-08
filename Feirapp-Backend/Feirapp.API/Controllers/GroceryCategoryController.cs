using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroceryCategoryController : ControllerBase
{
    private readonly IGroceryCategoryService _service;

    public GroceryCategoryController(IGroceryCategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<GroceryCategoryModel>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _service.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(GroceryCategoryModel), 200)]
    [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Invalid id");

        var result = await _service.GetByIdAsync(id, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GroceryCategoryModel), 201)]
    public async Task<IActionResult> Insert([FromBody] GroceryCategoryModel groceryCategoryModel, CancellationToken cancellationToken = default)
    {
        var result = await _service.InsertAsync(groceryCategoryModel, cancellationToken);
        return Created(nameof(GroceryCategoryModel), result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(AcceptedResult), 201)]
    public async Task<IActionResult> Update([FromBody] GroceryCategoryModel groceryCategoryModel, CancellationToken cancellationToken = default)
    {
        await _service.UpdateAsync(groceryCategoryModel, cancellationToken);
        return Accepted();
    }

    [HttpDelete("{id:length(24)}")]
    [ProducesResponseType(typeof(AcceptedResult), 201)]
    public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Invalid id");

        await _service.DeleteAsync(id, cancellationToken);
        return Accepted();
    }
}