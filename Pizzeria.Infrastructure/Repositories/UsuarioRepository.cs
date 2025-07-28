using Microsoft.EntityFrameworkCore;
using Pizzeria.Domain.Entities;
using Pizzeria.Domain.Interfaces;
using Pizzeria.Infrastructure.Data;
using Pizzeria.Infrastructure.Helper;


namespace Pizzeria.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly DataContext _context;

    public UsuarioRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Usuario>> GetUsuariosAsync(string? search, int pageNumber, int pageSize)
    {
        var query = _context.Usuarios.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            if (int.TryParse(search, out int dni))
            {
                query = query.Where(u => u.DNI == dni || u.Nombre.Contains(search));
            }
            else
            {
                query = query.Where(u => u.Nombre.Contains(search));
            }
        }

        return await query.PaginarAsync(pageNumber, pageSize);
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<Usuario> AddAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario> UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;
    }

}