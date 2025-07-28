

using Pizzeria.Domain.Entities;

namespace Pizzeria.Application.Interfaces;

public interface IUsuarioService
{
    Task<PagedResult<Usuario>> GetUsuariosAsync(string? search, int pageNumber, int pageSize);
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario> AddAsync(Usuario usuario);
    Task<Usuario?> UpdateAsync(int id, Usuario usuario);
    Task<bool> DeleteAsync(int id);
}