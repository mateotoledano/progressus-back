using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Dtos.NotificacionDtos;
using ProgressusWebApi.Models.NotificacionesModel;
using ProgressusWebApi.Request;
using ProgressusWebApi.Services.AuthServices;
using ProgressusWebApi.Services.AuthServices.Interfaces;
using ProgressusWebApi.Services.NotificacionesServices.Interfaces;

namespace ProgressusWebApi.Controllers.NotificacionControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesUsuarioController : ControllerBase
    {
        private readonly INotificacionesUsuariosService _notificacionesService;
        private readonly IAuthService _authService; // Usar la interfaz, no la clase concreta

        public NotificacionesUsuarioController(
            INotificacionesUsuariosService notificacionesService,
            IAuthService authService
        )
        {
            _notificacionesService = notificacionesService;
            _authService = authService;
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> ObtenerNotificacionesPorUsuario(string usuarioId)
        {
            // Obtener todos los usuarios entrenadores
            var usuariosEntrenadores = await _authService.ObtenerUsuariosEntrenadoresAsync();
            if (usuariosEntrenadores == null || !usuariosEntrenadores.Any())
            {
                return new NotFoundObjectResult(
                    "No se encontraron usuarios con el rol de Entrenador para notificar."
                );
            }

            // Verificar si el usuario actual es un entrenador
            var esEntrenador = usuariosEntrenadores.Any(u => u.IdentityUserId == usuarioId);

            // Obtener todas las notificaciones del usuario
            var notificaciones = await _notificacionesService.ObtenerNotificacionesPorUsuarioAsync(
                usuarioId
            );

            // Filtrar notificaciones si es entrenador
            if (esEntrenador)
            {
                notificaciones = notificaciones
                    .Where(n => n.Titulo == "Aviso: Máquina en mantenimiento")
                    .ToList();
            }
            else
            {
                var notificacionesPlantilla8 = notificaciones
                    .Where(n => n.Titulo == "¡Te extrañamos en el gimnasio!")
                    .GroupBy(n => n.FechaCreacion.Date)
                    .Select(g => g.OrderBy(n => n.FechaCreacion).First())
                    .ToList();

                var otrasNotificaciones = notificaciones
                    .Where(n => n.Titulo != "¡Te extrañamos en el gimnasio!")
                    .ToList();

                notificaciones = notificacionesPlantilla8
                    .Concat(otrasNotificaciones)
                    .OrderBy(n => n.FechaCreacion)
                    .ToList();
            }

            return Ok(notificaciones);
        }

        [HttpPost]
        public async Task<IActionResult> CrearNotificacion([FromBody] InsertNotificacionDto request)
        {
            var result = await _notificacionesService.CrearNotificacionAsync(
                request.PlantillaNotificacionId,
                request.UsuarioId
            );

            return result
                ? Ok(new { Message = "Notificación creada correctamente" })
                : BadRequest(new { Message = "Error al crear la notificación" });
        }

        [HttpPut("{notificacionId}")]
        public async Task<IActionResult> CambiarEstadoNotificacion(
            int notificacionId,
            [FromBody] CambiarEstadoRequest request
        )
        {
            var result = await _notificacionesService.CambiarEstadoNotificacionAsync(
                notificacionId,
                request.NuevoEstado
            );
            return result
                ? Ok(new { Message = "Estado de la notificación cambiado correctamente" })
                : BadRequest(new { Message = "Error al cambiar el estado de la notificación" });
        }

        [HttpDelete("{notificacionId}")]
        public async Task<IActionResult> EliminarNotificacion(int notificacionId)
        {
            var result = await _notificacionesService.EliminarNotificacionAsync(notificacionId);
            return result
                ? Ok(new { Message = "Notificación eliminada correctamente" })
                : BadRequest(new { Message = "Error al eliminar la notificación" });
        }

        [HttpPut("EnviarNotificacionesPendientes")]
        public async Task<IActionResult> EnviarNotificacionesPendientes()
        {
            var result = await _notificacionesService.EnviarNotificacionesPendientes();

            return result
                ? Ok(new { Message = "Notificaciones enviadas correctamente" })
                : BadRequest(new { Message = "Error al enviar las notificaciones" });
        }

        // Esta se usa para Descuentos y Promociones
        // También para Motivacional
        [HttpPost("CrearNotificacionMasiva")]
        public async Task<IActionResult> CrearNotificacionMasiva(
            [FromBody] NotificacionMasiva request
        )
        {
            var result = await _notificacionesService.CrearNotificacionMasivaAsync(request);

            return result
                ? Ok(new { Message = "Notificaciones masiva creada correctamente" })
                : BadRequest(new { Message = "Error al crear la notificación" });
        }
    }
}
