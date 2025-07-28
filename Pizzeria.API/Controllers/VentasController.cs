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
    private readonly IDetalleVentaService _detalleVentaService;

    public VentasController(
        IVentaService ventasService,
        IDetalleVentaService detalleVentaService)
    {
        _ventasService = ventasService;
        _detalleVentaService = detalleVentaService;
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

        var lista = result.Datos.Select(v => new
        {
            Id = v.Id,
            UsuarioId = v.IdUsuario,
            NombreUsuario = v.Usuario.Nombre,
            Fecha = v.Fecha,
            Total = v.Total
        });

        return Ok(new
        {
            result.TotalRegistros,
            result.PaginaActual,
            result.TamanoPagina,
            Datos = lista
        });
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

    // ---------------- DETALLES DE VENTA ----------------

    // GET /api/ventas/{ventaId}/detalles
    [HttpGet("{ventaId}/detalles")]
    public async Task<IActionResult> GetDetalles(int ventaId)
    {
        var detalles = await _detalleVentaService.GetDetallesByVentaIdAsync(ventaId);
        return Ok(detalles.Select(d => new
        {
            Id = d.Id,
            ProductoId = d.IdProducto,
            Cantidad = d.Cantidad,
            PrecioUnitario = d.PrecioUnitario,
            Total = d.Total
        }));
    }

    // GET /api/ventas/{ventaId}/detalles/{id}
    [HttpGet("{ventaId}/detalles/{id}")]
    public async Task<IActionResult> GetDetalle(int ventaId, int id)
    {
        var detalle = await _detalleVentaService.GetDetalleByIdAsync(ventaId, id);
        if (detalle == null)
            return NotFound(new { message = "Detalle no encontrado" });

        return Ok(new
        {
            Id = detalle.Id,
            ProductoId = detalle.IdProducto,
            Cantidad = detalle.Cantidad,
            PrecioUnitario = detalle.PrecioUnitario,
            Total = detalle.Total
        });
    }

    // PUT /api/ventas/{ventaId}/detalles/{id}
    [HttpPut("{ventaId}/detalles/{id}")]
    public async Task<IActionResult> UpdateDetalle(int ventaId, int id, [FromBody] DetalleVenta detalleVenta)
    {
        if (id != detalleVenta.Id || ventaId != detalleVenta.IdVentas)
            return BadRequest(new { message = "IDs de URL y cuerpo no coinciden" });

        var updated = await _detalleVentaService.UpdateDetalleAsync(ventaId, id, detalleVenta);

        return Ok(new
        {
            Id = updated.Id,
            ProductoId = updated.IdProducto,
            Cantidad = updated.Cantidad,
            PrecioUnitario = updated.PrecioUnitario,
            Total = updated.Total
        });
    }

    [HttpDelete("{ventaId}/detalles/{id}")]
    public async Task<IActionResult> DeleteDetalleVenta(int ventaId, int id)
    {
        var eliminado = await _detalleVentaService.DeleteDetalleVentaAsync(ventaId, id);
        if (!eliminado)
            return NotFound(new { message = $"No se encontró el detalle con ID {id} para la venta {ventaId}." });

        return NoContent();
    }
}