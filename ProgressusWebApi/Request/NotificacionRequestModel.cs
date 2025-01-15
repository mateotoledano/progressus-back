namespace ProgressusWebApi.Request
{
	public class CrearNotificacionRequest
	{
		public int PlantillaId { get; set; }
		public string UsuarioId { get; set; }
	}

	public class CambiarEstadoRequest
	{
		public int NotificacionId { get; set; }
		public string NuevoEstado { get; set; }
	}
}
