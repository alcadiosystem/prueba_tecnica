using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeria.Domain.Entities;

namespace Pizzeria.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<PagedResult<Usuario>> GetUsuariosAsync(string? search, int pageNumber, int pageSize);
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario> AddAsync(Usuario usuario);
    Task<Usuario> UpdateAsync(Usuario usuario);
    Task<bool> DeleteAsync(int id);
}