using Microsoft.EntityFrameworkCore;
using Pizzeria.Domain.Entities;
using Pizzeria.Domain.Interfaces;
using Pizzeria.Infrastructure.Data;

namespace Pizzeria.Infrastructure.Repositories;

public class DetalleVentaRepository : IDetalleVentaRepository
{
    private readonly DataContext _context;

    public DetalleVentaRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DetalleVenta>> GetDetallesByVentaIdAsync(int ventaId)
    {
        return await _context.DetalleVentas
            .Where(d => d.IdVentas == ventaId)
            .ToListAsync();
    }

    public async Task<DetalleVenta?> GetDetalleByIdAsync(int ventaId, int id)
    {
        return await _context.DetalleVentas
            .FirstOrDefaultAsync(d => d.IdVentas == ventaId && d.Id == id);
    }

    public async Task<DetalleVenta> UpdateDetalleAsync(DetalleVenta detalleVenta)
    {
        _context.DetalleVentas.Update(detalleVenta);
        await _context.SaveChangesAsync();
        return detalleVenta;
    }

    public async Task<DetalleVenta?> GetByIdAsync(int ventaId, int id)
    {
        return await _context.DetalleVentas
            .FirstOrDefaultAsync(d => d.IdVentas == ventaId && d.Id == id);
    }

    public async Task<bool> DeleteAsync(int ventaId, int id)
    {
        var detalle = await GetByIdAsync(ventaId, id);
        if (detalle == null) return false;

        _context.DetalleVentas.Remove(detalle);
        await _context.SaveChangesAsync();
        return true;
    }
}