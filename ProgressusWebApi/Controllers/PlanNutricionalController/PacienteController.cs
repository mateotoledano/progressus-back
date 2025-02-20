using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Models.PlanNutricional;

namespace ProgressusWebApi.Controllers.PlanNutricionalController
{
    public class PacienteController : Controller
    {
        private readonly ProgressusDataContext _context;

        public PacienteController(ProgressusDataContext context)
        {
            _context = context;
        }

        // GET: api/Pacientes/ObtenerTodos
        [HttpGet("ObtenerPaciente")]
        public async Task<ActionResult<IEnumerable<Paciente>>> ObtenerTodosLosPacientes()
        {
            return await _context.Pacientes.ToListAsync();
        }

        // GET: api/Pacientes/ObtenerPorId/5
        [HttpGet("ObtenerPacientePorId/{id}")]
        public async Task<ActionResult<Paciente>> ObtenerPacientePorId(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente == null)
            {
                return NotFound();
            }

            return paciente;
        }

        // POST: api/Pacientes/Crear
        [HttpPost("CrearPaciente")]
        public async Task<ActionResult<Paciente>> CrearPaciente(Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("ObtenerPacientePorId", new { id = paciente.Id }, paciente);
        }

        [HttpPut("ActualizarPaciente/{id}")]
        public async Task<IActionResult> ActualizarPaciente(int id, [FromBody] Paciente paciente)
        {
            if (id != paciente.Id)
            {
            //    _logger.LogWarning("El ID de la URL ({Id}) no coincide con el del objeto ({PacienteId})", id, paciente.Id);
                return BadRequest("ID en la URL no coincide con el del paciente.");
            }

            _context.Entry(paciente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
             //   _logger.LogInformation("Paciente {Id} actualizado correctamente.", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PacienteExists(id))
                {
              //      _logger.LogError("Intento de actualizar paciente {Id}, pero no existe en la base de datos.", id);
                    return NotFound("Paciente no encontrado.");
                }
                else
                {
                  //  _logger.LogError("Error de concurrencia al actualizar paciente {Id}.", id);
                    throw;
                }
            }

            return NoContent();
        }
        // DELETE: api/Pacientes/Eliminar/5
        [HttpDelete("EliminarPaciente/{id}")]
        public async Task<IActionResult> EliminarPaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.Id == id);
        }
    }
}