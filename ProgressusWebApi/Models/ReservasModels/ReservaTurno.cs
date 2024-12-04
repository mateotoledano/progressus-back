using ProgressusWebApi.Models.AsistenciaModels;
using ProgressusWebApi.Models.RerservasModels;

namespace ProgressusWebApi.Models.RerservasModels
{
    public class ReservaTurno
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Referencia al usuario
        public DateTime FechaReserva { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool Confirmada { get; set; }


   
    }
}