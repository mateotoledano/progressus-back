using ProgressusWebApi.Dtos.NotificacionDtos;
using ProgressusWebApi.Models.NotificacionesModel;

namespace ProgressusWebApi.Repositories.NotificacionesRepositories.Interfaces
{
	public interface IPlantillaRepository
	{
		Task<List<PlantillaNotificacion>> ObtenerPlantillasAsync();
		Task<PlantillaNotificacion> ObtenerPlantillaPorIdAsync(int plantillaId);


		Task<bool> CrearPlantillaAsync(PlantillaNotificacion plantilla);
		Task<bool> ActualizarPlantillaAsync(PlantillaNotificacion plantilla);
		Task<bool> EliminarPlantillaAsync(int plantillaId);
	}
}
