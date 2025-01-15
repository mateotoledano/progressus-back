using ProgressusWebApi.Dtos.NotificacionDtos;
using ProgressusWebApi.Models.NotificacionesModel;

namespace ProgressusWebApi.Services.NotificacionesServices.Interfaces
{
	public interface IPlantillasService
	{
		Task<List<PlantillaNotificacionDto>> ObtenerPlantillasAsync();
		Task<PlantillaNotificacionDto> ObtenerPlantillaPorIdAsync(int plantillaId);
		Task<bool> CrearPlantillaAsync(InsertPlantillaNotificacionDto plantilla);
		Task<bool> ActualizarPlantillaAsync(int plantillaId, UpdatePlantillaNotificacionDto plantilla);
		Task<bool> EliminarPlantillaAsync(int plantillaId);
	}
}
