using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Models.NotificacionesModel;
using ProgressusWebApi.Repositories.NotificacionesRepositories.Interfaces;

namespace ProgressusWebApi.Repositories.NotificacionesRepositories
{
	public class PlantillaRepository : IPlantillaRepository
	{
		private readonly ProgressusDataContext _context;

		public PlantillaRepository(ProgressusDataContext context)
		{
			_context = context;
		}

		public async Task<List<PlantillaNotificacion>> ObtenerPlantillasAsync()
		{
			return await _context.PlantillasNotificaciones.ToListAsync();
		}

		public async Task<PlantillaNotificacion> ObtenerPlantillaPorIdAsync(int plantillaId)
		{
			return await _context.PlantillasNotificaciones.FindAsync(plantillaId);
		}

		public async Task<bool> CrearPlantillaAsync(PlantillaNotificacion plantilla)
		{
			try
			{
				_context.PlantillasNotificaciones.Add(plantilla);
				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> ActualizarPlantillaAsync(PlantillaNotificacion plantilla)
		{
			try
			{
				_context.PlantillasNotificaciones.Update(plantilla);
				await _context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> EliminarPlantillaAsync(int plantillaId)
		{
			try
			{
				var plantilla = await _context.PlantillasNotificaciones.FindAsync(plantillaId);
				if (plantilla != null)
				{
					_context.PlantillasNotificaciones.Remove(plantilla);
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
