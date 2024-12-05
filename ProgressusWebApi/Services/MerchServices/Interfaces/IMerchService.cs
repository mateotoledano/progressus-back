using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Dtos.MerchDtos;

namespace ProgressusWebApi.Services.MerchServices.Interfaces
{
    public interface IMerchService
    {
        // Crear un nuevo merch
        Task<IActionResult> CrearMerchAsync(MerchDtos merchDto);

        // Obtener todos los merch
        Task<IActionResult> ObtenerTodosMerchAsync();

        // Obtener un merch por su Id
        Task<IActionResult> ObtenerMerchPorIdAsync(string id);

        // Editar un merch existente
        Task<IActionResult> EditarMerchAsync(string id, MerchDtos merchDto);

        // Eliminar un merch
        Task<IActionResult> EliminarMerchAsync(string id);
    }
}
