using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Dtos.NotificacionDtos;
using ProgressusWebApi.Services.NotificacionesServices.Interfaces;

namespace ProgressusWebApi.Controllers.NotificacionControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TiposNotificacionesController : ControllerBase
	{
		private readonly ITiposNotificacionesService _tiposNotificacionesService;

		public TiposNotificacionesController(ITiposNotificacionesService tiposNotificacionesService)
		{
			_tiposNotificacionesService = tiposNotificacionesService;
		}

		[HttpGet]
		public async Task<IActionResult> ObtenerTipos()
		{
			var tipos = await _tiposNotificacionesService.ObtenerTiposNotificacionesAsync();
			return Ok(tipos);
		}

		[HttpPost]
		public async Task<IActionResult> CrearTipo([FromBody] InsertTipoNotificacionDto tipo)
		{
			var result = await _tiposNotificacionesService.CrearTipoNotificacionAsync(tipo);
			return result ? Ok(new { Message = "Tipo de notificación creado exitosamente" }) : BadRequest(new { Message = "Error al crear el tipo de notificación" });
		}

		[HttpPut("{tipoId}")]
		public async Task<IActionResult> ActualizarTipo(int tipoId, [FromBody] UpdateTipoNotificacionDto tipo)
		{
			var result = await _tiposNotificacionesService.ActualizarTipoNotificacionAsync(tipoId, tipo);
			return result ? Ok(new { Message = "Tipo de notificación actualizado correctamente" }) : BadRequest(new { Message = "Error al actualizar el tipo de notificación" });
		}

		[HttpDelete("{tipoId}")]
		public async Task<IActionResult> EliminarTipo(int tipoId)
		{
			var result = await _tiposNotificacionesService.EliminarTipoNotificacionAsync(tipoId);
			return result ? Ok(new { Message = "Tipo de notificación eliminado correctamente" }) : BadRequest(new { Message = "Error al eliminar el tipo de notificación" });
		}
	}
}