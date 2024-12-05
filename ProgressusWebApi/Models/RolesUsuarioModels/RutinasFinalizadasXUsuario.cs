using System.ComponentModel.DataAnnotations;

namespace ProgressusWebApi.Models.RolesUsuarioModels
{
    public class RutinasFinalizadasXUsuario
    {
        [Key]
        public int IdRutina { get; set; }
        public string IdUser { get; set; }
        public bool HaFinalizado { get; set; }
    }
}
