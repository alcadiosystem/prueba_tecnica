using Microsoft.EntityFrameworkCore;
using Pizzeria.Domain.Entities;
using Pizzeria.Domain.Interfaces;
using Pizzeria.Infrastructure.Data;
using Pizzeria.Infrastructure.Helper;

namespace Pizzeria.Infrastructure.Repositories;

public class VentaRepository : IVentaRepository
{
    private readonly DataContext _context;

    public VentaRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Ventas>> GetVentasAsync(DateTime? fechaInicio, DateTime? fechaFin, string? search, int pageNumber, int pageSize)
    {
        var query = _context.Ventas
            .Include(v => v.Usuario)
            .AsQueryable();

        if (fechaInicio.HasValue)
            query = query.Where(v => v.Fecha >= fechaInicio.Value);

        if (fechaFin.HasValue)
            query = query.Where(v => v.Fecha <= fechaFin.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            if (int.TryParse(search, out int dni))
                query = query.Where(v => v.Usuario.DNI == dni || v.Usuario.Nombre.Contains(search));
            else
                query = query.Where(v => v.Usuario.Nombre.Contains(search));
        }

        return await query.PaginarAsync(pageNumber, pageSize);
    }

    public async Task<Ventas?> GetVentaByIdAsync(int id)
        => await _context.Ventas.Include(v => v.Usuario).FirstOrDefaultAsync(v => v.Id == id);

    public async Task<Ventas> CreateVentaAsync(Ventas venta)
    {
        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();
        return venta;
    }

    public async Task<Ventas> UpdateVentaAsync(Ventas venta)
    {
        _context.Ventas.Update(venta);
        await _context.SaveChangesAsync();
        return venta;
    }

    public async Task<bool> DeleteVentaAsync(int id)
    {
        var venta = await _context.Ventas
        .Include(v => v.DetalleVentas) // Incluye los detalles para poder eliminarlos
        .FirstOrDefaultAsync(v => v.Id == id);

        if (venta == null)
            return false;

        // Elimina los detalles primero
        if (venta.DetalleVentas != null && venta.DetalleVentas.Any())
        {
            _context.DetalleVentas.RemoveRange(venta.DetalleVentas);
        }

        // Elimina la venta
        _context.Ventas.Remove(venta);
        await _context.SaveChangesAsync();

        return true;
    }
}
