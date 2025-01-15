using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Dtos.NotificacionDtos;
using ProgressusWebApi.Services.NotificacionesServices.Interfaces;

namespace ProgressusWebApi.Controllers.NotificacionControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EstadosNotificacionesController : ControllerBase
	{
		private readonly IEstadosNotificacionesService _estadosNotificacionesService;

		public EstadosNotificacionesController(IEstadosNotificacionesService estadosNotificacionesService)
		{
			_estadosNotificacionesService = estadosNotificacionesService;
		}

		[HttpGet]
		public async Task<IActionResult> ObtenerEstados()
		{
			var estados = await _estadosNotificacionesService.ObtenerEstadosNotificacionesAsync();
			return Ok(estados);
		}

		[HttpPost]
		public async Task<IActionResult> CrearEstado([FromBody] InsertEstadoNotificacionDto estado)
		{
			await _estadosNotificacionesService.CrearEstadoNotificacionAsync(estado);
			return Ok(new { Message = "Estado de notificación creado exitosamente" });
		}

		[HttpPut("{estadoId}")]
		public async Task<IActionResult> ActualizarEstado(int estadoId, [FromBody] UpdateEstadoNotificacionDto estado)
		{
			await _estadosNotificacionesService.ActualizarEstadoNotificacionAsync(estadoId, estado);
			return Ok(new { Message = "Estado de notificación actualizado correctamente" });
		}

		[HttpDelete("{estadoId}")]
		public async Task<IActionResult> EliminarEstado(int estadoId)
		{
			await _estadosNotificacionesService.EliminarEstadoNotificacionAsync(estadoId);
			return Ok(new { Message = "Estado de notificación eliminado correctamente" });
		}
	}

}
