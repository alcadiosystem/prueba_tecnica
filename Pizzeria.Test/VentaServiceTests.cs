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
    public class VentaServiceTests
    {
        private Mock<IVentaRepository> _ventaRepoMock;
        private Mock<IUsuarioRepository> _usuarioRepoMock;
        private Mock<IProductoRepository> _productoRepoMock;
        private VentaService _ventaService;

        [TestInitialize]
        public void Setup()
        {
            _ventaRepoMock = new Mock<IVentaRepository>();
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _productoRepoMock = new Mock<IProductoRepository>();
            _ventaService = new VentaService(_ventaRepoMock.Object, _usuarioRepoMock.Object, _productoRepoMock.Object);
        }

        [TestMethod]
        public async Task GetVentasAsync_DeberiaRetornarVentasPaginadas()
        {
            // Arrange
            var ventas = new List<Ventas> { new Ventas { Id = 1, IdUsuario = 1, Total = 1000 } };
            _ventaRepoMock.Setup(r => r.GetVentasAsync(null, null, null, 1, 10))
                .ReturnsAsync(new PagedResult<Ventas> { Datos = ventas, PaginaActual = 1, TamanoPagina = 10, TotalRegistros = 1 });

            // Act
            var result = await _ventaService.GetVentasAsync(null, null, null, 1, 10);

            // Assert
            Assert.AreEqual(1, result.TotalRegistros);
            Assert.AreEqual(1000, result.Datos.First().Total);
        }

        [TestMethod]
        public async Task GetVentaByIdAsync_DeberiaRetornarVenta()
        {
            // Arrange
            var venta = new Ventas { Id = 1, IdUsuario = 1, Total = 500 };
            _ventaRepoMock.Setup(r => r.GetVentaByIdAsync(1)).ReturnsAsync(venta);

            // Act
            var result = await _ventaService.GetVentaByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task CreateVentaAsync_DeberiaCrearVentaYUsuario()
        {
            // Arrange
            var request = new CrearVentaRequest
            {
                NombreUsuario = "Juan",
                DNI = 1234,
                Detalles = new List<DetalleVentaItem> { new DetalleVentaItem { ProductoId = 1, Cantidad = 2 } }
            };

            _usuarioRepoMock.Setup(r => r.GetUsuariosAsync("1234", 1, 1))
                .ReturnsAsync(new PagedResult<Usuario> { Datos = new List<Usuario>() });

            _usuarioRepoMock.Setup(r => r.AddAsync(It.IsAny<Usuario>()))
                .ReturnsAsync(new Usuario { Id = 1, Nombre = "Juan", DNI = 1234 });

            _productoRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Producto { Id = 1, precio = 100 });

            _ventaRepoMock.Setup(r => r.CreateVentaAsync(It.IsAny<Ventas>()))
                .ReturnsAsync((Ventas v) => v);

            // Act
            var result = await _ventaService.CreateVentaAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.DetalleVentas.Count);
            Assert.AreEqual(200, result.Total);
        }

        [TestMethod]
        public async Task DeleteVentaAsync_DeberiaRetornarTrue()
        {
            _ventaRepoMock.Setup(r => r.DeleteVentaAsync(1)).ReturnsAsync(true);

            var result = await _ventaService.DeleteVentaAsync(1);

            Assert.IsTrue(result);
        }
    }