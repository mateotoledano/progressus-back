using System.ComponentModel.DataAnnotations;

namespace ProgressusWebApi.Models.PlanNutricional
{
    public class Comida
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TipoComida { get; set; } // desayuno, mediaMañana, almuerzo, etc.

        public int DiaPlanId { get; set; }
        public DiaPlan DiaPlan { get; set; }

        public ICollection<AlimentoComida> Alimentos { get; set; }
    }
}
