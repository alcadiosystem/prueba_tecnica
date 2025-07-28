using Pizzeria.Domain.Entities;

namespace Pizzeria.Application.Interfaces;

public interface IDetalleVentaService
{
    Task<IEnumerable<DetalleVenta>> GetDetallesByVentaIdAsync(int ventaId);
    Task<DetalleVenta?> GetDetalleByIdAsync(int ventaId, int id);
    Task<DetalleVenta> UpdateDetalleAsync(int ventaId, int id, DetalleVenta detalleVenta);

    Task<bool> DeleteDetalleVentaAsync(int ventaId, int id);
}