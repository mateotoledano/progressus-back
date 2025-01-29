using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.PlanesNutricionalesDtos;
using ProgressusWebApi.Models.PlanNutricional;

namespace ProgressusWebApi.Controllers.PlanNutricionalController
{
    public class AsignacionPlanNutricionalController : Controller
    {
        private readonly ProgressusDataContext _context;
        public AsignacionPlanNutricionalController(ProgressusDataContext context)
        {

            _context = context;
        }

        [HttpPost("asignar-plan")]
        public async Task<IActionResult> AsignarPlanNutricional([FromBody] AsignacionPlanNutricionalDto asignacionDto)
        {
            // Validar que el DTO no sea nulo
            if (asignacionDto == null)
            {
                return BadRequest("La asignación no puede ser nula.");
            }

            // Calcular la fecha de vigencia (1 mes después de la fecha actual)
            var fechaVigencia = DateTime.UtcNow.AddMonths(1);

            // Crear la asignación
            var asignacion = new AsignacionPlanNutricional
            {
                UsuarioId = asignacionDto.UsuarioId,
                PlanNutricionalId = asignacionDto.PlanNutricionalId,
                FechaVigencia = fechaVigencia,
                Activo = true
            };

            // Guardar la asignación en la base de datos
            _context.AsignacionesPlanNutricional.Add(asignacion);
            await _context.SaveChangesAsync();

            return Ok(asignacion);
        }

        [HttpGet("asignaciones-vigentes/{usuarioId}")]
        public async Task<IActionResult> ObtenerAsignacionesVigentes(string usuarioId)
        {
            // Obtener la fecha actual
            var fechaActual = DateTime.UtcNow;

            // Buscar las asignaciones vigentes del usuario
            var asignacionesVigentes = await _context.AsignacionesPlanNutricional
                .Include(a => a.PlanNutricional) // Incluir el plan nutricional
                .Where(a => a.UsuarioId == usuarioId && a.FechaVigencia >= fechaActual && a.Activo)
                .ToListAsync();

            return Ok(asignacionesVigentes);
        }

        [HttpPut("desactivar-asignacion/{id}")]
        public async Task<IActionResult> DesactivarAsignacion(int id)
        {
            // Buscar la asignación por ID
            var asignacion = await _context.AsignacionesPlanNutricional
                .FirstOrDefaultAsync(a => a.Id == id);

            if (asignacion == null)
            {
                return NotFound("Asignación no encontrada.");
            }

            // Desactivar la asignación
            asignacion.Activo = false;

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(asignacion);
        }
    }
}
