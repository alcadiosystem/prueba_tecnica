
using Pizzeria.Application.DTOs;
using Pizzeria.Domain.Entities;


namespace Pizzeria.Application.Interfaces;

public interface IProductoService
{
    Task<PagedResult<Producto>> GetProductosAsync(string? nombre, int pageNumber, int pageSize);
        Task<ProductoDto?> GetProductoByIdAsync(int id);
        Task<ProductoDto> CreateProductoAsync(ProductoDto productoDto);
        Task<ProductoDto> UpdateProductoAsync(ProductoDto productoDto);
        Task<bool> DeleteProductoAsync(int id);
}
