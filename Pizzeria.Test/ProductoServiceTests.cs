using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Pizzeria.Application.DTOs;
using Pizzeria.Application.Services;
using Pizzeria.Domain.Entities;
using Pizzeria.Domain.Interfaces;

namespace Pizzeria.Test;


[TestClass]
public class ProductoServiceTests
{
    private Mock<IProductoRepository> _productoRepositoryMock = null!;
    private ProductoService _productoService = null!;

    [TestInitialize]
    public void Setup()
    {
        _productoRepositoryMock = new Mock<IProductoRepository>();
        _productoService = new ProductoService(_productoRepositoryMock.Object);
    }

    [TestMethod]
    public async Task GetProductoById_DeberiaRetornarProducto()
    {
        var producto = new Producto { Id = 1, Nombre = "Pizza", Descripcion = "Pizza de queso", precio = 20000 };

        _productoRepositoryMock.Setup(r => r.GetByIdAsync(1))
                               .ReturnsAsync(producto);

        var result = await _productoService.GetProductoByIdAsync(1);

        Assert.IsNotNull(result);
        Assert.AreEqual("Pizza", result.Nombre);
    }

    [TestMethod]
    public async Task GetProductoById_DeberiaRetornarNull_SiNoExiste()
    {
        _productoRepositoryMock.Setup(r => r.GetByIdAsync(2))
                               .ReturnsAsync((Producto)null);

        var result = await _productoService.GetProductoByIdAsync(2);

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task CreateProductoAsync_DeberiaCrearProducto()
    {
        var productoDto = new ProductoDto { Nombre = "Pizza", Descripcion = "Pizza de queso", Precio = 20000 };

        _productoRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Producto>()))
                               .ReturnsAsync((Producto p) =>
                               {
                                   p.Id = 1;
                                   return p;
                               });

        var result = await _productoService.CreateProductoAsync(productoDto);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        Assert.AreEqual("Pizza", result.Nombre);
    }

    [TestMethod]
    public async Task UpdateProductoAsync_DeberiaActualizarProducto()
    {
        var productoDto = new ProductoDto { Id = 1, Nombre = "Pizza Actualizada", Descripcion = "Pizza de jamón", Precio = 25000 };

        _productoRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Producto>()))
                               .ReturnsAsync((Producto p) =>
                               {
                                   // Simula que la BD guarda la actualización
                                   return new Producto
                                   {
                                       Id = p.Id,
                                       Nombre = p.Nombre,
                                       Descripcion = p.Descripcion,
                                       precio = p.precio
                                   };
                               });

        var result = await _productoService.UpdateProductoAsync(productoDto);

        Assert.IsNotNull(result);
        Assert.AreEqual("Pizza Actualizada", result.Nombre);
    }

    [TestMethod]
    public async Task DeleteProductoAsync_DeberiaEliminarProducto()
    {
        _productoRepositoryMock.Setup(r => r.DeleteAsync(1))
                               .ReturnsAsync(true);

        var result = await _productoService.DeleteProductoAsync(1);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task DeleteProductoAsync_DeberiaRetornarFalse_SiNoExiste()
    {
        _productoRepositoryMock.Setup(r => r.DeleteAsync(2))
                               .ReturnsAsync(false);

        var result = await _productoService.DeleteProductoAsync(2);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task GetProductosAsync_DeberiaRetornarListaPaginada()
    {
        var productos = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Pizza", Descripcion = "Pizza de queso", precio = 20000 }
            };

        var pagedResult = new PagedResult<Producto>
        {
            TotalRegistros = 1,
            PaginaActual = 1,
            TamanoPagina = 10,
            Datos = productos
        };

        _productoRepositoryMock.Setup(r => r.GetProductosAsync(null, 1, 10))
                               .ReturnsAsync(pagedResult);

        var result = await _productoService.GetProductosAsync(null, 1, 10);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.TotalRegistros);
        Assert.AreEqual("Pizza", result.Datos[0].Nombre);
    }
}