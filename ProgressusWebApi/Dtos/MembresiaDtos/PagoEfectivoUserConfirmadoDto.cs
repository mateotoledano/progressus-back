namespace ProgressusWebApi.Dtos.MembresiaDtos
{
    public class PagoEfectivoUserConfirmadoDto
    {
        public int TipoMembresiaId { get; set; }
        public string NombreMembresia { get; set; } // Nombre de la membresía
        public decimal PrecioMembresia { get; set; } // Precio de la membresía
        public DateTime FechaPago { get; set; }
        public string IdentityUserId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }

}