
using Moq;
using Pizzeria.Application.Services;
using Pizzeria.Domain.Entities;
using Pizzeria.Domain.Interfaces;

namespace Pizzeria.Test;

[TestClass]
    public class DetalleVentaServiceTests
    {
        private Mock<IDetalleVentaRepository> _repositoryMock;
        private DetalleVentaService _service;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IDetalleVentaRepository>();
            _service = new DetalleVentaService(_repositoryMock.Object);
        }

        [TestMethod]
        public async Task GetDetallesByVentaIdAsync_DeberiaRetornarLista()
        {
            // Arrange
            var detalles = new List<DetalleVenta>
            {
                new DetalleVenta { Id = 1, IdVentas = 1, IdProducto = 1, Cantidad = 2, PrecioUnitario = 100 }
            };
            _repositoryMock.Setup(r => r.GetDetallesByVentaIdAsync(1)).ReturnsAsync(detalles);

            // Act
            var result = await _service.GetDetallesByVentaIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, ((List<DetalleVenta>)result).Count);
        }

        [TestMethod]
        public async Task GetDetalleByIdAsync_DeberiaRetornarDetalle()
        {
            // Arrange
            var detalle = new DetalleVenta { Id = 1, IdVentas = 1, IdProducto = 2, Cantidad = 3, PrecioUnitario = 200 };
            _repositoryMock.Setup(r => r.GetDetalleByIdAsync(1, 1)).ReturnsAsync(detalle);

            // Act
            var result = await _service.GetDetalleByIdAsync(1, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(1, result.IdVentas);
        }

        [TestMethod]
        public async Task UpdateDetalleAsync_DeberiaActualizarDetalle()
        {
            // Arrange
            var detalle = new DetalleVenta { Id = 1, IdVentas = 1, IdProducto = 2, Cantidad = 3, PrecioUnitario = 150 };
            _repositoryMock.Setup(r => r.UpdateDetalleAsync(detalle)).ReturnsAsync(detalle);

            // Act
            var result = await _service.UpdateDetalleAsync(1, 1, detalle);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(150, result.PrecioUnitario);
        }

        [TestMethod]
        public async Task DeleteDetalleVentaAsync_DeberiaRetornarTrue()
        {
            // Arrange
            _repositoryMock.Setup(r => r.DeleteAsync(1, 1)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteDetalleVentaAsync(1, 1);

            // Assert
            Assert.IsTrue(result);
        }
    }