namespace ProgressusWebApi.Models.NotificacionModel
{
    public class Notificacion
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string Mensaje { get; set; }
        public bool Estado { get; set; }
    }
}
