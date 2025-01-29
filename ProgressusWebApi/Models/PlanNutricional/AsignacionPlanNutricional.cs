using Microsoft.AspNetCore.Identity;

namespace ProgressusWebApi.Models.PlanNutricional
{
    public class AsignacionPlanNutricional
    {
        public int Id { get; set; }

        // Relación con AspNetUsers
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }

        // Relación con PlanNutricional
        public int PlanNutricionalId { get; set; }
        public PlanNutricional PlanNutricional { get; set; }

        public DateTime FechaVigencia { get; set; } // Fecha de expiración del plan
        public bool Activo { get; set; } = true; // Por defecto, la asignación está activa
    }
}
