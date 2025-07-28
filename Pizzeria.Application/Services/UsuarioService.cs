using Pizzeria.Application.Interfaces;
using Pizzeria.Domain.Entities;
using Pizzeria.Domain.Interfaces;

namespace Pizzeria.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<PagedResult<Usuario>> GetUsuariosAsync(string? search, int pageNumber, int pageSize)
    {
        return await _usuarioRepository.GetUsuariosAsync(search, pageNumber, pageSize);
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _usuarioRepository.GetByIdAsync(id);
    }

    public async Task<Usuario> AddAsync(Usuario usuario)
    {
        return await _usuarioRepository.AddAsync(usuario);
    }

    public async Task<Usuario?> UpdateAsync(int id, Usuario usuario)
    {
        var existing = await _usuarioRepository.GetByIdAsync(id);
        if (existing == null) return null;

        existing.Nombre = usuario.Nombre;
        existing.DNI = usuario.DNI;

        return await _usuarioRepository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _usuarioRepository.DeleteAsync(id);
    }

}