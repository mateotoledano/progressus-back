using ProgressusWebApi.Models.NotificacionesModel;

namespace ProgressusWebApi.Repositories.NotificacionesRepositories.Interfaces
{
	public interface ITipoNotificacionRepository
	{
		Task<List<TipoNotificacion>> ObtenerTiposNotificacionesAsync();
		Task<TipoNotificacion> ObtenerTipoNotificacionPorIdAsync(int tipoId);
		Task<bool> CrearTipoNotificacionAsync(TipoNotificacion tipo);
		Task<bool> ActualizarTipoNotificacionAsync(TipoNotificacion tipo);
		Task<bool> EliminarTipoNotificacionAsync(int tipoId);
	}
}
