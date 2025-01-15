using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Models.NotificacionModel;
using ProgressusWebApi.Services.NotificacionesServices.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgressusWebApi.Services.NotificacionesServices
{
	public class NotificacionesService : INotificacionesService
	{
		private readonly ProgressusDataContext _context;

		public NotificacionesService(ProgressusDataContext context)
		{
			_context = context;
		}

		public async Task<List<Notificacion>> ObtenerNotificacionesPorUsuarioAsync(string usuarioId)
		{
			return await _context.Notificaciones
								 .Where(n => n.UsuarioId == usuarioId)
								 .ToListAsync();
		}

		public async Task<bool> MarcarNotificacionComoLeidaAsync(int id)
		{
			// Buscar la notificación
			var notificacion = await _context.Notificaciones.FindAsync(id);

			if (notificacion == null || notificacion.Estado == true)
			{
				// Retorna false si no existe o ya está leída
				return false;
			}

			// Marcar la notificación como leída
			notificacion.Estado = true;
			_context.Notificaciones.Update(notificacion);
			await _context.SaveChangesAsync();

			return true;
		}

	}
}
