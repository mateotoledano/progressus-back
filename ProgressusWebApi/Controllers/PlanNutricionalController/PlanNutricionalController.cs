using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.PlanesNutricionalesDtos;
using ProgressusWebApi.Models.PlanNutricional;
using ProgressusWebApi.Services.ReservaService.cs.interfaces;
using Microsoft.EntityFrameworkCore;

namespace ProgressusWebApi.Controllers.PlanNutricionalController
{
    public class PlanNutricionalController : Controller
    {
        private readonly ProgressusDataContext _context;

        public PlanNutricionalController( ProgressusDataContext context)
        {
       
            _context = context;
        }

        [HttpPost("crear-plan")]
        public async Task<IActionResult> CrearPlanNutricional([FromBody] PlanNutricionalDto planDto)
        {
            // Validar que el DTO no sea nulo
            if (planDto == null)
            {
                return BadRequest("El plan nutricional no puede ser nulo.");
            }

            // Validar que la lista de días no sea nula o vacía
            if (planDto.Dias == null || !planDto.Dias.Any())
            {
                return BadRequest("El plan nutricional debe contener al menos un día.");
            }

            // Crear el plan nutricional
            var plan = new PlanNutricional
            {
                Nombre = planDto.Nombre,
                Dias = planDto.Dias.Select(d => new DiaPlan
                {
                    Dia = d.Dia ?? "Día no especificado", // Validar que el día no sea nulo
                    Comidas = d.Comidas?.Select(c => new Comida
                    {
                        TipoComida = c.TipoComida ?? "Comida no especificada", // Validar que el tipo de comida no sea nulo
                        Alimentos = c.Alimentos?.Select(a => new AlimentoComida
                        {
                            AlimentoId = a.AlimentoId,
                            Cantidad = a.Cantidad,
                            Medida = a.Medida ?? "Medida no especificada" // Validar que la medida no sea nula
                        }).ToList() ?? new List<AlimentoComida>() // Si Alimentos es nulo, usar una lista vacía
                    }).ToList() ?? new List<Comida>() // Si Comidas es nulo, usar una lista vacía
                }).ToList()
            };

            // Guardar el plan en la base de datos
            _context.PlanesNutricionales.Add(plan);
            await _context.SaveChangesAsync();

            return Ok(plan);
        }

        [HttpGet("obtener-plan/{id}")]
        public async Task<IActionResult> ObtenerPlanNutricional(int id)
        {
            var plan = await _context.PlanesNutricionales
                .Include(p => p.Dias)
                    .ThenInclude(d => d.Comidas)
                        .ThenInclude(c => c.Alimentos)
                            .ThenInclude(a => a.Alimento)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
            {
                return NotFound();
            }

            var planDto = new PlanNutricionalDto
            {
                Id = plan.Id,
                Nombre = plan.Nombre,
                Dias = plan.Dias.Select(d => new DiaPlanDto
                {
                    Dia = d.Dia,
                    Comidas = d.Comidas.Select(c => new ComidaDto
                    {
                        TipoComida = c.TipoComida,
                        Alimentos = c.Alimentos.Select(a => new AlimentoComidaDto
                        {
                            AlimentoId = a.AlimentoId,
                            AlimentoNombre = a.Alimento.Nombre,
                            Cantidad = a.Cantidad,
                            Medida = a.Medida
                        }).ToList()
                    }).ToList()
                }).ToList()
            };

            return Ok(planDto);
        }

        [HttpGet("obtener-planes")]
        public async Task<IActionResult> ObtenerTodosLosPlanes()
        {
            var planes = await _context.PlanesNutricionales
                .Include(p => p.Dias)
                    .ThenInclude(d => d.Comidas)
                        .ThenInclude(c => c.Alimentos)
                            .ThenInclude(a => a.Alimento)
                .ToListAsync();

            if (planes == null || !planes.Any())
            {
                return NotFound();
            }

            var planesDto = planes.Select(plan => new PlanNutricionalDto
            {
                Id = plan.Id,
                Nombre = plan.Nombre,
                Dias = plan.Dias.Select(d => new DiaPlanDto
                {
                    Dia = d.Dia,
                    Comidas = d.Comidas.Select(c => new ComidaDto
                    {
                        TipoComida = c.TipoComida,
                        Alimentos = c.Alimentos.Select(a => new AlimentoComidaDto
                        {
                            AlimentoId = a.AlimentoId,
                            AlimentoNombre = a.Alimento.Nombre,
                            Cantidad = a.Cantidad,
                            Medida = a.Medida
                        }).ToList()
                    }).ToList()
                }).ToList()
            }).ToList();

            return Ok(planesDto);
        }

        [HttpPut("actualizar-plan/{id}")]
        public async Task<IActionResult> ActualizarPlanNutricional(int id, [FromBody] PlanNutricionalDto planDto)
        {
            // Validar que el DTO no sea nulo
            if (planDto == null)
            {
                return BadRequest("El plan nutricional no puede ser nulo.");
            }

            // Buscar el plan existente en la base de datos
            var planExistente = await _context.PlanesNutricionales
                .Include(p => p.Dias)
                    .ThenInclude(d => d.Comidas)
                        .ThenInclude(c => c.Alimentos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (planExistente == null)
            {
                return NotFound("Plan nutricional no encontrado.");
            }

            // Actualizar el nombre del plan
            planExistente.Nombre = planDto.Nombre;

            // Eliminar los días existentes (y sus comidas y alimentos asociados)
            _context.DiasPlan.RemoveRange(planExistente.Dias);

            // Agregar los nuevos días
            planExistente.Dias = planDto.Dias.Select(d => new DiaPlan
            {
                Dia = d.Dia ?? "Día no especificado",
                Comidas = d.Comidas?.Select(c => new Comida
                {
                    TipoComida = c.TipoComida ?? "Comida no especificada",
                    Alimentos = c.Alimentos?.Select(a => new AlimentoComida
                    {
                        AlimentoId = a.AlimentoId,
                        Cantidad = a.Cantidad,
                        Medida = a.Medida ?? "Medida no especificada"
                    }).ToList() ?? new List<AlimentoComida>()
                }).ToList() ?? new List<Comida>()
            }).ToList();

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(planExistente);
        }

        [HttpDelete("eliminar-plan/{id}")]
        public async Task<IActionResult> EliminarPlanNutricional(int id)
        {
            // Buscar el plan existente en la base de datos
            var planExistente = await _context.PlanesNutricionales
                .Include(p => p.Dias)
                    .ThenInclude(d => d.Comidas)
                        .ThenInclude(c => c.Alimentos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (planExistente == null)
            {
                return NotFound("Plan nutricional no encontrado.");
            }

            // Eliminar el plan y sus relaciones (días, comidas y alimentos)
            _context.PlanesNutricionales.Remove(planExistente);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

    }
}
