using Pizzeria.Application.Interfaces;
using Pizzeria.Domain.Entities;
using Pizzeria.Domain.Interfaces;

namespace Pizzeria.Application.Services;

public class DetalleVentaService : IDetalleVentaService
{

    private readonly IDetalleVentaRepository _repository;

    public DetalleVentaService(IDetalleVentaRepository repository)
    {
        _repository = repository;
    }

    public DetalleVentaService()
    {
    }

    public async Task<IEnumerable<DetalleVenta>> GetDetallesByVentaIdAsync(int ventaId)
    {
        return await _repository.GetDetallesByVentaIdAsync(ventaId);
    }

    public async Task<DetalleVenta?> GetDetalleByIdAsync(int ventaId, int id)
    {
        return await _repository.GetDetalleByIdAsync(ventaId, id);
    }

    public async Task<DetalleVenta> UpdateDetalleAsync(int ventaId, int id, DetalleVenta detalleVenta)
    {
        if (detalleVenta.Id != id || detalleVenta.IdVentas != ventaId)
            throw new ArgumentException("Los IDs no coinciden");

        return await _repository.UpdateDetalleAsync(detalleVenta);
    }

    public async Task<bool> DeleteDetalleVentaAsync(int ventaId, int id)
    {
        return await _repository.DeleteAsync(ventaId, id);
    }

}