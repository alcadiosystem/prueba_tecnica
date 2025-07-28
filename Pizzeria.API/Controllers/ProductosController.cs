
using Microsoft.AspNetCore.Mvc;
using Pizzeria.Application.DTOs;
using Pizzeria.Application.Interfaces;

namespace Pizzeria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{

    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
        public async Task<IActionResult> GetProductos(
            [FromQuery] string? name,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
        try
        {
            var productos = await _productoService.GetProductosAsync(name, pageNumber, pageSize);
            return Ok(productos);
        }
        catch (System.Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
        public async Task<IActionResult> GetProductoById(int id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducto([FromBody] ProductoDto productoDto)
        {
            var created = await _productoService.CreateProductoAsync(productoDto);
            return CreatedAtAction(nameof(GetProductoById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, [FromBody] ProductoDto productoDto)
        {
            if (id != productoDto.Id)
                return BadRequest("El ID de la URL no coincide con el del cuerpo.");

            var updated = await _productoService.UpdateProductoAsync(productoDto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var deleted = await _productoService.DeleteProductoAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

}
