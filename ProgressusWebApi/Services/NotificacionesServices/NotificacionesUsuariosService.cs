using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.NotificacionDtos;
using ProgressusWebApi.Models.NotificacionesModel;
using ProgressusWebApi.Repositories.NotificacionesRepositories.Interfaces;
using ProgressusWebApi.Services.NotificacionesServices.Interfaces;

namespace ProgressusWebApi.Services.NotificacionesServices
{
	public class NotificacionesUsuariosService : INotificacionesUsuariosService
	{
		private readonly INotificacionRepository _notificacionRepository;
		private readonly IPlantillaRepository _plantillaRepository;
		private readonly IEstadoNotificacionRepository _estadoNotificacionRepository;
		private readonly ProgressusDataContext _context;

		public NotificacionesUsuariosService(INotificacionRepository notificacionRepository,IPlantillaRepository plantillaRepository,IEstadoNotificacionRepository estadoNotificacionRepository, ProgressusDataContext context)
		{
			_notificacionRepository = notificacionRepository;
			_plantillaRepository = plantillaRepository;
			_estadoNotificacionRepository = estadoNotificacionRepository;
			_context = context;
		}

		public async Task<List<NotificacionDto>> ObtenerNotificacionesPorUsuarioAsync(string usuarioId)
		{
			var notificaciones = await _notificacionRepository.ObtenerNotificacionesPorUsuarioAsync(usuarioId);
			return notificaciones.Select(n => new NotificacionDto
			{
				Id = n.Id,
				Titulo = n.PlantillaNotificacion.Titulo,
				Cuerpo = n.Cuerpo,
				Estado = n.EstadoNotificacion.Nombre,
				Usuario = n.UsuarioId,
				FechaCreacion = n.FechaCreacion,
				FechaEnvio = n.FechaEnvio
			}).ToList();
		}

		public async Task<bool> CrearNotificacionAsync(int plantillaId, string usuarioId)
		{
			var plantilla = await _plantillaRepository.ObtenerPlantillaPorIdAsync(plantillaId);
			if (plantilla == null)
				throw new Exception("Plantilla no encontrada");


			var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == usuarioId);

			if (usuario == null)
				throw new Exception("Usuario no encontrado");

			// Personalizar el contenido de la notificación
			string cuerpoPersonalizado = plantilla.Cuerpo
				.Replace("[Nombre]", usuario.UserName ?? "Usuario");


			var estadoPendiente = await _estadoNotificacionRepository
				.ObtenerEstadosNotificacionesAsync()
				.ContinueWith(t => t.Result.FirstOrDefault(e => e.Nombre == "Pendiente"));

			if (estadoPendiente == null)
				throw new Exception("Estado 'Pendiente' no encontrado");


			var notificacion = new Notificacion
			{
				PlantillaNotificacionId = plantilla.Id,
				UsuarioId = usuarioId,
				EstadoNotificacionId = estadoPendiente.Id,
				FechaCreacion = DateTime.UtcNow,
				Titulo = plantilla.Titulo,
				Cuerpo = cuerpoPersonalizado
			};

			return await _notificacionRepository.CrearNotificacionAsync(notificacion);
		}

		public async Task<bool> CambiarEstadoNotificacionAsync(int notificacionId, string nuevoEstado)
		{
			return await _notificacionRepository.CambiarEstadoNotificacionAsync(notificacionId, nuevoEstado);
		}

		public async Task<bool> EliminarNotificacionAsync(int notificacionId)
		{
			return await _notificacionRepository.EliminarNotificacionAsync(notificacionId);
		}
	}

}
