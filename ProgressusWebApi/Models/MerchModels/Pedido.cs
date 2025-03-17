using Microsoft.AspNetCore.Identity;

namespace ProgressusWebApi.Models.MerchModels
{
    public class Pedido
    {
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public string CarritoId { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public Carrito Carrito { get; set; }
        public IdentityUser Usuario { get; set; }
    }
}
