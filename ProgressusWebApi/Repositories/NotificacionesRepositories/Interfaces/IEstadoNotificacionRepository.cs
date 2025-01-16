using ProgressusWebApi.Models.NotificacionesModel;

namespace ProgressusWebApi.Repositories.NotificacionesRepositories.Interfaces
{
	public interface IEstadoNotificacionRepository
	{
		Task<List<EstadoNotificacion>> ObtenerEstadosNotificacionesAsync();
		Task<bool> CrearEstadoNotificacionAsync(EstadoNotificacion estado);
		Task<bool> ActualizarEstadoNotificacionAsync(EstadoNotificacion estado);
		Task<bool> EliminarEstadoNotificacionAsync(int estadoId);
	}
}
