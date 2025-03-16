using Microsoft.AspNetCore.Identity;

namespace ProgressusWebApi.Models.MerchModels
{
    public class Carrito
    {
        public string Id { get; set; } // Clave primaria
        public string UsuarioId { get; set; } // Relación con el usuario
        public List<CarritoItem> Items { get; set; } = new List<CarritoItem>(); // Relación con CarritoItem
        public decimal Total { get; set; } // Total del carrito

        public IdentityUser Usuario { get; set; }
    }
}
