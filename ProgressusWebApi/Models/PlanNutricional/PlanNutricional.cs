using ProgressusWebApi.Model;
using System.ComponentModel.DataAnnotations;

namespace ProgressusWebApi.Models.PlanNutricional
{
    public class PlanNutricional
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public ICollection<DiaPlan> Dias { get; set; }
    }
}
