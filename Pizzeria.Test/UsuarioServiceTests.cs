
using Moq;
using Pizzeria.Application.Services;
using Pizzeria.Domain.Entities;
using Pizzeria.Domain.Interfaces;

namespace Pizzeria.Test;

[TestClass]
public class UsuarioServiceTests
{
    private Mock<IUsuarioRepository> _usuarioRepositoryMock = null!;
    private UsuarioService _usuarioService = null!;

    [TestInitialize]
    public void Setup()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object);
    }

    [TestMethod]
    public async Task GetUsuariosAsync_DeberiaRetornarListaPaginada()
    {
        var pagedResult = new PagedResult<Usuario>
        {
            TotalRegistros = 1,
            PaginaActual = 1,
            TamanoPagina = 10,
            Datos = new List<Usuario> { new Usuario { Id = 1, Nombre = "Juan", DNI = 12345 } }
        };

        _usuarioRepositoryMock.Setup(r => r.GetUsuariosAsync(null, 1, 10))
                              .ReturnsAsync(pagedResult);

        var result = await _usuarioService.GetUsuariosAsync(null, 1, 10);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.TotalRegistros);
        Assert.AreEqual("Juan", result.Datos[0].Nombre);
    }

    [TestMethod]
    public async Task GetByIdAsync_DeberiaRetornarUsuario()
    {
        var usuario = new Usuario { Id = 1, Nombre = "Carlos", DNI = 54321 };
        _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usuario);

        var result = await _usuarioService.GetByIdAsync(1);

        Assert.IsNotNull(result);
        Assert.AreEqual("Carlos", result.Nombre);
    }

    [TestMethod]
    public async Task AddAsync_DeberiaCrearUsuario()
    {
        var nuevoUsuario = new Usuario { Nombre = "Pedro", DNI = 112233 };
        _usuarioRepositoryMock.Setup(r => r.AddAsync(nuevoUsuario))
                              .ReturnsAsync(new Usuario { Id = 1, Nombre = "Pedro", DNI = 112233 });

        var result = await _usuarioService.AddAsync(nuevoUsuario);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        Assert.AreEqual("Pedro", result.Nombre);
    }

    [TestMethod]
    public async Task UpdateAsync_DeberiaActualizarUsuario()
    {
        var usuarioExistente = new Usuario { Id = 1, Nombre = "Pedro", DNI = 112233 };
        var usuarioActualizado = new Usuario { Nombre = "Pedro Modificado", DNI = 445566 };

        _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usuarioExistente);
        _usuarioRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Usuario>()))
                              .ReturnsAsync((Usuario u) => u);

        var result = await _usuarioService.UpdateAsync(1, usuarioActualizado);

        Assert.IsNotNull(result);
        Assert.AreEqual("Pedro Modificado", result.Nombre);
        Assert.AreEqual(445566, result.DNI);
    }

    [TestMethod]
    public async Task UpdateAsync_DeberiaRetornarNull_SiUsuarioNoExiste()
    {
        _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Usuario?)null);

        var result = await _usuarioService.UpdateAsync(999, new Usuario { Nombre = "Nuevo", DNI = 123 });

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task DeleteAsync_DeberiaRetornarTrue_SiUsuarioEliminado()
    {
        _usuarioRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _usuarioService.DeleteAsync(1);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task DeleteAsync_DeberiaRetornarFalse_SiUsuarioNoExiste()
    {
        _usuarioRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(false);

        var result = await _usuarioService.DeleteAsync(1);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task GetByIdAsync_DeberiaRetornarNull_SiUsuarioNoExiste()
    {
        _usuarioRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Usuario?)null);

        var result = await _usuarioService.GetByIdAsync(999);

        Assert.IsNull(result);
    }
}