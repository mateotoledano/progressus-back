using ProgressusWebApi.Dtos.NotificacionDtos;

namespace ProgressusWebApi.Services.NotificacionesServices.Interfaces
{
	public interface ITiposNotificacionesService
	{
		Task<List<TipoNotificacionDto>> ObtenerTiposNotificacionesAsync();
		Task<bool> CrearTipoNotificacionAsync(InsertTipoNotificacionDto tipo);
		Task<bool> ActualizarTipoNotificacionAsync(int tipoId, UpdateTipoNotificacionDto tipo);
		Task<bool> EliminarTipoNotificacionAsync(int tipoId);
	}
}
