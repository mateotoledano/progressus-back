using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProgressusWebApi.Models.RolesUsuarioModels
{
    public class MedicionesUsuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Indica que el Id es autogenerado
        public int Id { get; set; }

        public string IdUser { get; set; }

        public decimal Altura { get; set; }

        public decimal Peso { get; set; }

        public decimal PorcentajeDeGrasa { get; set; }

        public DateTime Fecha { get; set; }
    }
}
