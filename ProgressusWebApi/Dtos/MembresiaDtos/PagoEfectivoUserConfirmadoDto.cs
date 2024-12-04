namespace ProgressusWebApi.Dtos.MembresiaDtos
{
    public class PagoEfectivoUserConfirmadoDto
    {
        public int TipoMembresiaId { get; set; }
        public DateTime FechaPago { get; set; }
        public string? IdentityUserId { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
    }
}