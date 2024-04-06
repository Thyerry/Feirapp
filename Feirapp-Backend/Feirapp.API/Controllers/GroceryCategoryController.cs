using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Dtos;
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
    [ProducesResponseType(typeof(List<GroceryCategoryDto>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _service.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(GroceryCategoryDto), 200)]
    [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Invalid id");

        var result = await _service.GetByIdAsync(id, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GroceryCategoryDto), 201)]
    public async Task<IActionResult> Insert([FromBody] GroceryCategoryDto groceryCategoryDto,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.InsertAsync(groceryCategoryDto, cancellationToken);
        return Created(nameof(GroceryCategoryDto), result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(AcceptedResult), 201)]
    public async Task<IActionResult> Update([FromBody] GroceryCategoryDto groceryCategoryDto,
        CancellationToken cancellationToken = default)
    {
        await _service.UpdateAsync(groceryCategoryDto, cancellationToken);
        return Accepted();
    }

    [HttpDelete("{id:length(24)}")]
    [ProducesResponseType(typeof(AcceptedResult), 201)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Invalid id");

        await _service.DeleteAsync(id, cancellationToken);
        return Accepted();
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<GroceryCategoryDto>), 200)]
    public async Task<IActionResult> Search([FromQuery] GroceryCategoryDto groceryCategory,
        CancellationToken cancellationToken)
    {
        return Ok(await _service.SearchAsync(groceryCategory, cancellationToken));
    }

    [HttpPost("batch")]
    [ProducesResponseType(typeof(List<GroceryCategoryDto>), 201)]
    public async Task<IActionResult> InsertBatch(List<GroceryCategoryDto> groceryCategoryDtos,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.InsertBatch(groceryCategoryDtos, cancellationToken);
        return Created(nameof(List<GroceryCategoryDto>), result);
    }
}