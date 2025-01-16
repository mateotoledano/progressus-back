namespace ProgressusWebApi.Dtos.NotificacionDtos
{
	public class NotificacionDto
	{
		public int Id { get; set; }
		public string Titulo { get; set; }
		public string Cuerpo { get; set; }
		public string Estado { get; set; } = "Pendiente";
		public string Usuario { get; set; }
		public DateTime FechaCreacion { get; set; }
		public DateTime? FechaEnvio { get; set; }
	}

}
