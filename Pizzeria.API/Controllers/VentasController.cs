using Microsoft.AspNetCore.Mvc;
using Pizzeria.Application.DTOs;
using Pizzeria.Application.Interfaces;
using Pizzeria.Domain.Entities;

namespace Pizzeria.API.Controllers;

[ApiController]
[Route("api/ventas")]
public class VentasController : ControllerBase
{
    private readonly IVentaService _ventasService;

    public VentasController(IVentaService ventasService)
    {
        _ventasService = ventasService;
    }

    [HttpGet]
    public async Task<IActionResult> GetVentas(
        [FromQuery] DateTime? fechaInicio,
        [FromQuery] DateTime? fechaFin,
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _ventasService.GetVentasAsync(fechaInicio, fechaFin, search, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVentaById(int id)
    {
        var venta = await _ventasService.GetVentaByIdAsync(id);
        if (venta == null)
            return NotFound();

        return Ok(new
        {
            Id = venta.Id,
            UsuarioId = venta.IdUsuario,
            NombreUsuario = venta.Usuario.Nombre,
            Fecha = venta.Fecha,
            Total = venta.Total
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateVenta([FromBody] CrearVentaRequest request)
    {
        var venta = await _ventasService.CreateVentaAsync(request);
        return Ok(new
        {
            Id = venta.Id,
            UsuarioId = venta.IdUsuario,
            Fecha = venta.Fecha,
            Total = venta.Total,
            Detalles = venta.DetalleVentas.Select(d => new
            {
                d.Id,
                d.IdProducto,
                d.Cantidad,
                d.PrecioUnitario,
                Total = d.Cantidad * d.PrecioUnitario
            })
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVenta(int id, [FromBody] Ventas venta)
    {
        if (id != venta.Id) return BadRequest();

        var updated = await _ventasService.UpdateVentaAsync(venta);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVenta(int id)
    {
        var result = await _ventasService.DeleteVentaAsync(id);
        if (!result) return NotFound();

        return Ok(new { message = $"La venta con ID {id} fue eliminada correctamente." });
    }
}