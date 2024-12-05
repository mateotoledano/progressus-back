using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.MerchDtos;
using ProgressusWebApi.Models.MerchModels;
using ProgressusWebApi.Services.MerchServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ProgressusWebApi.Services.MerchServices
{
    public class MerchService : IMerchService
    {
        private readonly ProgressusDataContext _context;

        public MerchService(ProgressusDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CrearMerchAsync(MerchDtos merchDto)
        {
            var merch = new Merch
            {
                Nombre = merchDto.Nombre,
                Descripcion = merchDto.Descripcion,
                Precio = merchDto.Precio
            };

            _context.Merch.Add(merch);
            await _context.SaveChangesAsync();

            return new OkObjectResult(merch);
        }

        public async Task<IActionResult> ObtenerTodosMerchAsync()
        {
            var merch = await _context.Merch.ToListAsync();
            return new OkObjectResult(merch);
        }

        public async Task<IActionResult> ObtenerMerchPorIdAsync(string id)
        {
            var merch = await _context.Merch.FindAsync(int.Parse(id));
            if (merch == null)
            {
                return new NotFoundObjectResult("Merch no encontrado.");
            }

            return new OkObjectResult(merch);
        }

        public async Task<IActionResult> EditarMerchAsync(string id, MerchDtos merchDto)
        {
            var merch = await _context.Merch.FindAsync(int.Parse(id));
            if (merch == null)
            {
                return new NotFoundObjectResult("Merch no encontrado.");
            }

            merch.Nombre = merchDto.Nombre;
            merch.Descripcion = merchDto.Descripcion;
            merch.Precio = merchDto.Precio;

            _context.Merch.Update(merch);
            await _context.SaveChangesAsync();

            return new OkObjectResult(merch);
        }

        public async Task<IActionResult> EliminarMerchAsync(string id)
        {
            // Convertir id de string a int
            if (!int.TryParse(id, out int parsedId))
            {
                return new BadRequestObjectResult("El id proporcionado no es válido.");
            }

            var merch = await _context.Merch.FindAsync(parsedId);
            if (merch == null)
            {
                return new NotFoundObjectResult("Merch no encontrado.");
            }

            _context.Merch.Remove(merch);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Merch eliminado.");
        }


    }
}
