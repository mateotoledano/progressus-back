using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Models.NotificacionesModel;
using ProgressusWebApi.Repositories.NotificacionesRepositories.Interfaces;

namespace ProgressusWebApi.Repositories.NotificacionesRepositories
{
	public class TipoNotificacionRepository : ITipoNotificacionRepository
	{
		private readonly ProgressusDataContext _context;

		public TipoNotificacionRepository(ProgressusDataContext context)
		{
			_context = context;
		}

		public async Task<List<TipoNotificacion>> ObtenerTiposNotificacionesAsync()
		{
			return await _context.TiposNotificaciones.ToListAsync();
		}

		public async Task<bool> CrearTipoNotificacionAsync(TipoNotificacion tipo)
		{
			try
			{
				_context.TiposNotificaciones.Add(tipo);
				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> ActualizarTipoNotificacionAsync(TipoNotificacion tipo)
		{

			try
			{
				_context.TiposNotificaciones.Update(tipo);
				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> EliminarTipoNotificacionAsync(int tipoId)
		{
			try
			{
				var tipo = await _context.TiposNotificaciones.FindAsync(tipoId);
				if (tipo == null)
				{
					return false;
				}
				_context.TiposNotificaciones.Remove(tipo);
				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<TipoNotificacion> ObtenerTipoNotificacionPorIdAsync(int tipoId)
		{
			return await _context.TiposNotificaciones.FindAsync(tipoId);
		}
	}
}
