using ProgressusWebApi.Models.AlimentosModels;
using System.ComponentModel.DataAnnotations;

namespace ProgressusWebApi.Models.PlanNutricional
{
    public class AlimentoComida
    {
        [Key]
        public int Id { get; set; }

        public int ComidaId { get; set; }
        public Comida Comida { get; set; }

        public int AlimentoId { get; set; }
        public Alimento Alimento { get; set; }

        public int Cantidad { get; set; }

        [StringLength(50)]
        public string Medida { get; set; }
    }
}
