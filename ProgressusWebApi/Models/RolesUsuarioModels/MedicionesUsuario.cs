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

        public double Altura { get; set; }

        public double Peso { get; set; }

        public double PorcentajeDeGrasa { get; set; }

        public DateTime Fecha { get; set; }
    }
}
