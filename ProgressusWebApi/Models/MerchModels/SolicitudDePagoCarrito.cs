using System.Security.Policy;

namespace ProgressusWebApi.Models.MerchModels
{
    public class SolicitudDePagoCarrito
    {
        public string UsuarioId { get; set; }
        public Carrito Carrito { get; set; }
    }
}
