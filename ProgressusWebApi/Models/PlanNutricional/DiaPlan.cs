using System.ComponentModel.DataAnnotations;

namespace ProgressusWebApi.Models.PlanNutricional
{
    public class DiaPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Dia { get; set; }

        public int PlanNutricionalId { get; set; }
        public PlanNutricional PlanNutricional { get; set; }

        public ICollection<Comida> Comidas { get; set; }
    }
}
