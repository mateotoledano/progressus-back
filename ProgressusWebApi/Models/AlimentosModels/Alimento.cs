using System.ComponentModel.DataAnnotations;

namespace ProgressusWebApi.Models.AlimentosModels
{
    public class Alimento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public int Porcion { get; set; }

        [Required]
        public int Calorias { get; set; }

        [Required]
        public decimal Carbohidratos { get; set; }

        [Required]
        public decimal Proteinas { get; set; }

        [Required]
        public decimal Grasas { get; set; }
    }
}
