using ProgressusWebApi.Models.RerservasModels;

namespace ProgressusWebApi.Models.AsistenciaModels
{
    public class AsistenciaLog
    {
        public int Id { get; set; }                // Clave primaria
        public string UserId { get; set; }         // Clave foránea para IdentityUser
        public int ReservaId { get; set; }         // Clave foránea para ReservaTurno
        public DateTime FechaAsistencia { get; set; }

        // Relación con ReservaTurno
        public virtual ReservaTurno ReservaTurno { get; set; }
    }
}
