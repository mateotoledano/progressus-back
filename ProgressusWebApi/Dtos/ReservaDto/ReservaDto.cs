namespace ProgressusWebApi.Dtos.RerservaDto
{
    public class RerservaDto
    {
        public string UserId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool Confirmada { get; set; }

    }
}