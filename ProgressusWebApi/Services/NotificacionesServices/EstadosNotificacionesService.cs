using ProgressusWebApi.Dtos.NotificacionDtos;
using ProgressusWebApi.Models.NotificacionesModel;
using ProgressusWebApi.Repositories.NotificacionesRepositories.Interfaces;
using ProgressusWebApi.Services.NotificacionesServices.Interfaces;

namespace ProgressusWebApi.Services.NotificacionesServices
{
	public class EstadosNotificacionesService : IEstadosNotificacionesService
	{
		private readonly IEstadoNotificacionRepository _estadoNotificacionRepository;

		public EstadosNotificacionesService(IEstadoNotificacionRepository estadoNotificacionRepository)
		{
			_estadoNotificacionRepository = estadoNotificacionRepository;
		}

		public async Task<List<EstadoNotificacionDto>> ObtenerEstadosNotificacionesAsync()
		{
			var estados = await _estadoNotificacionRepository.ObtenerEstadosNotificacionesAsync();
			return estados.Select(e => new EstadoNotificacionDto
			{
				Id = e.Id,
				Nombre = e.Nombre,
				Descripcion = e.Descripcion
			}).ToList();
		}

		public async Task<bool> CrearEstadoNotificacionAsync(InsertEstadoNotificacionDto estadoDto)
		{
			var estado = new EstadoNotificacion
			{
				Nombre = estadoDto.Nombre,
				Descripcion = estadoDto.Descripcion
			};
			return await _estadoNotificacionRepository.CrearEstadoNotificacionAsync(estado);
		}

		public async Task<bool> ActualizarEstadoNotificacionAsync(int estadoId, UpdateEstadoNotificacionDto estadoDto)
		{
			var estado = await _estadoNotificacionRepository.ObtenerEstadosNotificacionesAsync()
				.ContinueWith(t => t.Result.FirstOrDefault(x => x.Id == estadoId));

			if (estado == null)
				throw new Exception("Estado no encontrado");

			estado.Nombre = estadoDto.Nombre;
			estado.Descripcion = estadoDto.Descripcion;

			return await _estadoNotificacionRepository.ActualizarEstadoNotificacionAsync(estado);
		}

		public async Task<bool> EliminarEstadoNotificacionAsync(int estadoId)
		{
			return await _estadoNotificacionRepository.EliminarEstadoNotificacionAsync(estadoId);
		}
	}

}
