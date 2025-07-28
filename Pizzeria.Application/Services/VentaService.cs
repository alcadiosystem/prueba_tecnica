
using Pizzeria.Application.DTOs;
using Pizzeria.Application.Interfaces;
using Pizzeria.Domain.Entities;
using Pizzeria.Domain.Interfaces;

namespace Pizzeria.Application.Services;

public class VentaService : IVentaService
{
    private readonly IVentaRepository _ventasRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IProductoRepository _productoRepository;


    public VentaService(IVentaRepository ventasRepository, IUsuarioRepository usuarioRepository, IProductoRepository productoRepository)
    {
        _ventasRepository = ventasRepository;
        _usuarioRepository = usuarioRepository;
        _productoRepository = productoRepository;
    }

    public async Task<PagedResult<Ventas>> GetVentasAsync(DateTime? fechaInicio, DateTime? fechaFin, string? search, int pageNumber, int pageSize)
    {
        return await _ventasRepository.GetVentasAsync(fechaInicio, fechaFin, search, pageNumber, pageSize);
    }

    public async Task<Ventas?> GetVentaByIdAsync(int id)
    {
        return await _ventasRepository.GetVentaByIdAsync(id);
    }

    public async Task<Ventas> CreateVentaAsync(CrearVentaRequest request)
    {
        // 1. Buscar usuario existente usando búsqueda por nombre o DNI
        var usuarios = await _usuarioRepository.GetUsuariosAsync(request.DNI.ToString(), 1, 1);
        var usuario = usuarios.Datos.FirstOrDefault();

        if (usuario == null)
        {
            // Crear usuario si no existe
            usuario = await _usuarioRepository.AddAsync(new Usuario
            {
                Nombre = request.NombreUsuario,
                DNI = request.DNI
            });
        }

        // 2. Crear la venta
        var venta = new Ventas
        {
            IdUsuario = usuario.Id,
            Fecha = DateTime.UtcNow,
            Total = 0m,
            DetalleVentas = new List<DetalleVenta>()
        };

        // 3. Agregar detalles
        foreach (var item in request.Detalles)
        {
            var producto = await _productoRepository.GetByIdAsync(item.ProductoId);
            if (producto == null)
                throw new Exception($"Producto con ID {item.ProductoId} no encontrado.");

            var detalle = new DetalleVenta
            {
                IdProducto = producto.Id,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.precio
            };

            venta.DetalleVentas.Add(detalle);
            venta.Total += detalle.Cantidad * detalle.PrecioUnitario;
        }

        // 4. Guardar venta
        return await _ventasRepository.CreateVentaAsync(venta);
    }

    public async Task<Ventas> UpdateVentaAsync(Ventas venta)
    {
        return await _ventasRepository.UpdateVentaAsync(venta);
    }

    public async Task<bool> DeleteVentaAsync(int id)
    {
        return await _ventasRepository.DeleteVentaAsync(id);
    }
}