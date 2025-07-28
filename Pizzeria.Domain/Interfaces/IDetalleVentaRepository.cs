
using Pizzeria.Domain.Entities;

namespace Pizzeria.Domain.Interfaces;

public interface IDetalleVentaRepository
{
    Task<IEnumerable<DetalleVenta>> GetDetallesByVentaIdAsync(int ventaId);
    Task<DetalleVenta?> GetDetalleByIdAsync(int ventaId, int id);
    Task<DetalleVenta> UpdateDetalleAsync(DetalleVenta detalleVenta);
    Task<DetalleVenta?> GetByIdAsync(int ventaId, int id);
    Task<bool> DeleteAsync(int ventaId, int id);
}