
using Microsoft.EntityFrameworkCore;
using Pizzeria.Domain.Entities;
using Pizzeria.Domain.Interfaces;
using Pizzeria.Infrastructure.Data;
using Pizzeria.Infrastructure.Helper;

namespace Pizzeria.Infrastructure.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly DataContext _context;

    public ProductoRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<PagedResult<Producto>> GetProductosAsync(string? nombre, int pageNumber, int pageSize)
    {
        var query = _context.Productos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(p => p.Nombre.Contains(nombre));

        return await query.PaginarAsync(pageNumber, pageSize);
    }
    public async Task<Producto> AddAsync(Producto producto)
    {
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        return producto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null) return false;

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Producto>> GetAllAsync(string? name = null)
    {
        var query = _context.Productos.AsQueryable();
        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Nombre.Contains(name));

        return await query.ToListAsync();
    }

    public async Task<Producto?> GetByIdAsync(int id)
    {
        return await _context.Productos.FindAsync(id);
    }

    public async Task<Producto> UpdateAsync(Producto producto)
    {
        _context.Productos.Update(producto);
        await _context.SaveChangesAsync();
        return producto;
    }
}
