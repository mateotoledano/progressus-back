using Microsoft.AspNetCore.Identity;

namespace ProgressusWebApi.Models.NotificacionesModel
{
	public class Notificacion
	{
		public int Id { get; set; }
		public int PlantillaNotificacionId { get; set; }
		public string UsuarioId { get; set; }
		public int EstadoNotificacionId { get; set; }
		public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
		public DateTime? FechaEnvio { get; set; } = null;
		public string Titulo { get; set; } 
		public string Cuerpo { get; set; }

		public PlantillaNotificacion PlantillaNotificacion { get; set; }
		public EstadoNotificacion EstadoNotificacion { get; set; }
		public IdentityUser Usuario { get; set; }
	}
}
