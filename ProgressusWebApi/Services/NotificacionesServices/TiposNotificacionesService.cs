using ProgressusWebApi.Dtos.NotificacionDtos;
using ProgressusWebApi.Models.NotificacionesModel;
using ProgressusWebApi.Repositories.NotificacionesRepositories.Interfaces;
using ProgressusWebApi.Services.NotificacionesServices.Interfaces;

namespace ProgressusWebApi.Services.NotificacionesServices
{
	public class TiposNotificacionesService : ITiposNotificacionesService
	{
		private readonly ITipoNotificacionRepository _tipoNotificacionRepository;

		public TiposNotificacionesService(ITipoNotificacionRepository tipoNotificacionRepository)
		{
			_tipoNotificacionRepository = tipoNotificacionRepository;
		}

		public async Task<List<TipoNotificacionDto>> ObtenerTiposNotificacionesAsync()
		{
			var tipos = await _tipoNotificacionRepository.ObtenerTiposNotificacionesAsync();
			return tipos.Select(t => new TipoNotificacionDto
			{
				Id = t.Id,
				Nombre = t.Nombre,
				Descripcion = t.Descripcion
			}).ToList();
		}

		public async Task<bool> CrearTipoNotificacionAsync(InsertTipoNotificacionDto tipoDto)
		{
			var tipo = new TipoNotificacion
			{
				Nombre = tipoDto.Nombre,
				Descripcion = tipoDto.Descripcion
			};
			return await _tipoNotificacionRepository.CrearTipoNotificacionAsync(tipo);
		}

		public async Task<bool> ActualizarTipoNotificacionAsync(int tipoId, UpdateTipoNotificacionDto tipoDto)
		{
			var tipo = await _tipoNotificacionRepository.ObtenerTipoNotificacionPorIdAsync(tipoId);

			if (tipo == null)
				throw new Exception("Tipo no encontrado");

			tipo.Nombre = tipoDto.Nombre;
			tipo.Descripcion = tipoDto.Descripcion;

			return await _tipoNotificacionRepository.ActualizarTipoNotificacionAsync(tipo);
		}

		public async Task<bool> EliminarTipoNotificacionAsync(int tipoId)
		{
			return await _tipoNotificacionRepository.EliminarTipoNotificacionAsync(tipoId);
		}
	}
}
