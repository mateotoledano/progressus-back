using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Dtos.InventarioDtos;

namespace ProgressusWebApi.Services.InventarioServices.Interfaces
{
    public interface IInventarioService
    {
        // Crear un nuevo inventario
        Task<IActionResult> CrearInventarioAsync(InventarioDtos inventarioDto);

        // Obtener todos los inventarios
        Task<IActionResult> ObtenerTodosInventariosAsync();

        // Obtener un inventario por su Id
        Task<IActionResult> ObtenerInventarioPorIdAsync(string id);

        // Editar un inventario existente
        Task<IActionResult> EditarInventarioAsync(string id, InventarioDtos inventarioDto);

        // Eliminar un inventario
        Task<IActionResult> EliminarInventarioAsync(string id);

        // Obtener todos los inventarios en Mantenimiento
        Task<IActionResult> ObtenerInventarioMantenimientoAsync();
    }
}
