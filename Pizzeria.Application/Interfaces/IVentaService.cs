using Pizzeria.Application.DTOs;
using Pizzeria.Domain.Entities;

namespace Pizzeria.Application.Interfaces;

public interface IVentaService
{
    Task<PagedResult<Ventas>> GetVentasAsync(DateTime? fechaInicio, DateTime? fechaFin, string? search, int pageNumber, int pageSize);
    Task<Ventas?> GetVentaByIdAsync(int id);
    Task<Ventas> CreateVentaAsync(CrearVentaRequest request);
    Task<Ventas> UpdateVentaAsync(Ventas venta);
    Task<bool> DeleteVentaAsync(int id);
}