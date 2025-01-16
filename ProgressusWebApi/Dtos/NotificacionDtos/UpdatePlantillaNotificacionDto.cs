namespace ProgressusWebApi.Dtos.NotificacionDtos
{
	public class UpdatePlantillaNotificacionDto
	{
		public int TipoNotificacionId { get; set; }
		public string RolId { get; set; }
		public string Titulo { get; set; }
		public string Cuerpo { get; set; }
		public string? DiaSemana { get; set; }
		public string? MomentoDia { get; set; }
	}
}
