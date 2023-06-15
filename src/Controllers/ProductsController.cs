using Microsoft.AspNetCore.Mvc;
using Thinktecture.Webinars.SampleApi.Models;
using Thinktecture.Webinars.SampleApi.Services;

namespace Thinktecture.Webinars.SampleApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductsService _service;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ProductsService service, ILogger<ProductsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var all = await _service.GetAllProducts();
        return Ok(all);
    }

    [HttpGet("{id:guid}", Name = "GetProductById")]
    public async Task<IActionResult> GetProductById([FromRoute] Guid id)
    {
        var found = await _service.GetProductById(id);
        if (found == null)
        {
            return NotFound();
        }

        return Ok(found);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var created = await _service.CreateProduct(model);
        return CreatedAtRoute("GetProductById", new { id = created.Id }, created);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id)
    {
        var deleted = await _service.DeleteProductById(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
