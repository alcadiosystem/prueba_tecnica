
using Pizzeria.Domain.Entities;

namespace Pizzeria.Domain.Interfaces;

public interface IProductoRepository
{
    Task<PagedResult<Producto>> GetProductosAsync(string? nombre, int pageNumber, int pageSize);
    Task<IEnumerable<Producto>> GetAllAsync(string? name = null);
    Task<Producto?> GetByIdAsync(int id);
    Task<Producto> AddAsync(Producto producto);
    Task<Producto> UpdateAsync(Producto producto);
    Task<bool> DeleteAsync(int id);
}
