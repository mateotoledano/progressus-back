using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.InventarioDtos;
using ProgressusWebApi.Services.InventarioServices.Interfaces;
using ProgressusWebApi.Models.InventarioModels;
using Microsoft.AspNetCore.Mvc;

namespace ProgressusWebApi.Services.InventarioServices
{
    public class InventarioService : IInventarioService
    {
        private readonly ProgressusDataContext _context;

        public InventarioService(ProgressusDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CrearInventarioAsync(InventarioDtos inventarioDto)
        {
            var inventario = new Inventario
            {
                Nombre = inventarioDto.Nombre,
                Descripcion = inventarioDto.Descripcion,
                Estado = inventarioDto.Estado
            };

            _context.Inventario.Add(inventario);
            await _context.SaveChangesAsync();

            return new OkObjectResult(inventario);
        }

        public async Task<IActionResult> ObtenerTodosInventariosAsync()
        {
            var inventarios = await _context.Inventario.ToListAsync();
            return new OkObjectResult(inventarios);
        }

        public async Task<IActionResult> ObtenerInventarioPorIdAsync(string id)
        {
            var inventario = await _context.Inventario.FindAsync(int.Parse(id));
            if (inventario == null)
            {
                return new NotFoundObjectResult("Inventario no encontrado.");
            }

            return new OkObjectResult(inventario);
        }

        public async Task<IActionResult> EditarInventarioAsync(string id, InventarioDtos inventarioDto)
        {
            var inventario = await _context.Inventario.FindAsync(int.Parse(id));
            if (inventario == null)
            {
                return new NotFoundObjectResult("Inventario no encontrado.");
            }

            inventario.Nombre = inventarioDto.Nombre;
            inventario.Descripcion = inventarioDto.Descripcion;
            inventario.Estado = inventarioDto.Estado;

            _context.Inventario.Update(inventario);
            await _context.SaveChangesAsync();

            return new OkObjectResult(inventario);
        }

        public async Task<IActionResult> EliminarInventarioAsync(string id)
        {
            // Convertir id de string a int
            if (!int.TryParse(id, out int parsedId))
            {
                return new BadRequestObjectResult("El id proporcionado no es válido.");
            }

            var inventario = await _context.Inventario.FindAsync(parsedId);
            if (inventario == null)
            {
                return new NotFoundObjectResult("Inventario no encontrado.");
            }

            _context.Inventario.Remove(inventario);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Inventario eliminado.");
        }


    }
}
