using ProgressusWebApi.Dtos.NotificacionDtos;
using ProgressusWebApi.Models.NotificacionesModel;

namespace ProgressusWebApi.Services.NotificacionesServices.Interfaces
{
	public interface IEstadosNotificacionesService
	{
		Task<List<EstadoNotificacionDto>> ObtenerEstadosNotificacionesAsync();
		Task<bool> CrearEstadoNotificacionAsync(InsertEstadoNotificacionDto estado);
		Task<bool> ActualizarEstadoNotificacionAsync(int estadoId, UpdateEstadoNotificacionDto estado);
		Task<bool> EliminarEstadoNotificacionAsync(int estadoId);
	}
}
