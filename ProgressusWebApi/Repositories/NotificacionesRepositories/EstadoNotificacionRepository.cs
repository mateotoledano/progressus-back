using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Models.NotificacionesModel;
using ProgressusWebApi.Repositories.NotificacionesRepositories.Interfaces;

namespace ProgressusWebApi.Repositories.NotificacionesRepositories
{
	public class EstadoNotificacionRepository : IEstadoNotificacionRepository
	{
		private readonly ProgressusDataContext _context;

		public EstadoNotificacionRepository(ProgressusDataContext context)
		{
			_context = context;
		}

		public async Task<List<EstadoNotificacion>> ObtenerEstadosNotificacionesAsync()
		{
			return await _context.EstadosNotificaciones.ToListAsync();
		}

		public async Task<bool> CrearEstadoNotificacionAsync(EstadoNotificacion estado)
		{
			try
			{
				_context.EstadosNotificaciones.Add(estado);
				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> ActualizarEstadoNotificacionAsync(EstadoNotificacion estado)
		{
			try
			{
				_context.EstadosNotificaciones.Update(estado);
				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> EliminarEstadoNotificacionAsync(int estadoId)
		{
			try
			{
				var estado = await _context.EstadosNotificaciones.FindAsync(estadoId);
				if (estado != null)
				{
					_context.EstadosNotificaciones.Remove(estado);
					await _context.SaveChangesAsync();
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}
	}
}
