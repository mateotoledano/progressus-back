using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Services.NotificacionesServices.Interfaces;

namespace ProgressusWebApi.Controllers.NotificacionesControllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionesService _notificacionesService;

        public NotificacionesController(INotificacionesService notificacionesService)
        {
            _notificacionesService = notificacionesService;  // Asignación correcta
        }

        // Obtener notificaciones por usuario
        [HttpGet("ObtenerPorUsuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerPorUsuario(string usuarioId)
        {
            if (string.IsNullOrEmpty(usuarioId))
            {
                return BadRequest("El ID del usuario es obligatorio.");
            }

            var notificaciones = await _notificacionesService.ObtenerNotificacionesPorUsuarioAsync(usuarioId);

            if (notificaciones == null || notificaciones.Count == 0)
            {
                return NotFound("No se encontraron notificaciones para este usuario.");
            }

            return Ok(notificaciones);
        }

        [HttpPut("MarcarComoLeida/{id}")]
        public async Task<IActionResult> MarcarComoLeida(int id)
        {
            try
            {
                var resultado = await _notificacionesService.MarcarNotificacionComoLeidaAsync(id);

                if (!resultado)
                {
                    return NotFound($"No se encontró la notificación con ID {id} o ya está marcada como leída.");
                }

                return Ok($"La notificación con ID {id} ha sido marcada como leída.");
            }
            catch (Exception ex)
            {
                // Loguear el error
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

    }
}
