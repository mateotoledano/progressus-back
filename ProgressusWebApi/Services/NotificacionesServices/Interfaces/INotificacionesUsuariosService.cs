using ProgressusWebApi.Dtos.NotificacionDtos;

namespace ProgressusWebApi.Services.NotificacionesServices.Interfaces
{
	public interface INotificacionesUsuariosService
	{
		Task<List<NotificacionDto>> ObtenerNotificacionesPorUsuarioAsync(string usuarioId);
		Task<bool> CrearNotificacionAsync(int plantillaId, string usuarioId);
		Task<bool> CambiarEstadoNotificacionAsync(int notificacionId, string nuevoEstado);
		Task<bool> EliminarNotificacionAsync(int notificacionId);
	}
}
