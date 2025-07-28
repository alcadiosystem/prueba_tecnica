

using Pizzeria.Application.DTOs;
using Pizzeria.Application.Interfaces;
using Pizzeria.Domain.Entities;
using Pizzeria.Domain.Interfaces;

namespace Pizzeria.Application.Services;

public class ProductoService : IProductoService
{

    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<ProductoDto> CreateProductoAsync(ProductoDto productoDto)
    {
        var producto = new Producto
        {
            Nombre = productoDto.Nombre,
            Descripcion = productoDto.Descripcion,
            precio = productoDto.Precio
        };

        var created = await _productoRepository.AddAsync(producto);

        productoDto.Id = created.Id;
        return productoDto;
    }

    public async Task<bool> DeleteProductoAsync(int id)
    {
        return await _productoRepository.DeleteAsync(id);
    }

    public async Task<ProductoDto?> GetProductoByIdAsync(int id)
    {
        var producto = await _productoRepository.GetByIdAsync(id);
        if (producto == null) return null;

        return new ProductoDto
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Precio = producto.precio
        };
    }

    public async Task<PagedResult<Producto>> GetProductosAsync(string? nombre, int pageNumber, int pageSize)
    {
        return await _productoRepository.GetProductosAsync(nombre, pageNumber, pageSize);
    }

    public async Task<ProductoDto> UpdateProductoAsync(ProductoDto productoDto)
    {
        var producto = new Producto
        {
            Id = productoDto.Id,
            Nombre = productoDto.Nombre,
            Descripcion = productoDto.Descripcion,
            precio = productoDto.Precio
        };

        var updated = await _productoRepository.UpdateAsync(producto);
        return new ProductoDto
        {
            Id = updated.Id,
            Nombre = updated.Nombre,
            Descripcion = updated.Descripcion,
            Precio = updated.precio
        };
    }
}
