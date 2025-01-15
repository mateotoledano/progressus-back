using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Dtos.NotificacionDtos;
using ProgressusWebApi.Services.NotificacionesServices.Interfaces;

namespace ProgressusWebApi.Controllers.NotificacionControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlantillasNotificacionesController : ControllerBase
	{
		private readonly IPlantillasService _plantillasService;

		public PlantillasNotificacionesController(IPlantillasService plantillasService)
		{
			_plantillasService = plantillasService;
		}

		[HttpGet]
		public async Task<IActionResult> ObtenerPlantillas()
		{
			var plantillas = await _plantillasService.ObtenerPlantillasAsync();
			return Ok(plantillas);
		}

		[HttpGet("{plantillaId}")]
		public async Task<IActionResult> ObtenerPlantillaPorId(int plantillaId)
		{
			var plantilla = await _plantillasService.ObtenerPlantillaPorIdAsync(plantillaId);
			return Ok(plantilla);
		}

		[HttpPost]
		public async Task<IActionResult> CrearPlantilla([FromBody] InsertPlantillaNotificacionDto plantilla)
		{
			var result = await _plantillasService.CrearPlantillaAsync(plantilla);
			return result ? Ok(new { Message = "Plantilla creada correctamente" }) : BadRequest(new { Message = "Error al crear la plantilla" });
		}

		[HttpPut("{plantillaId}")]
		public async Task<IActionResult> ActualizarPlantilla(int plantillaId, [FromBody] UpdatePlantillaNotificacionDto plantilla)
		{
			var result = await _plantillasService.ActualizarPlantillaAsync(plantillaId, plantilla);
			return result ? Ok(new { Message = "Plantilla actualizada correctamente" }) : BadRequest(new { Message = "Error al actualizar la plantilla" });
		}

		[HttpDelete("{plantillaId}")]
		public async Task<IActionResult> EliminarPlantilla(int plantillaId)
		{
			var result = await _plantillasService.EliminarPlantillaAsync(plantillaId);
			return result ? Ok(new { Message = "Plantilla eliminada correctamente" }) : BadRequest(new { Message = "Error al eliminar la plantilla" });
		}
	}
}
