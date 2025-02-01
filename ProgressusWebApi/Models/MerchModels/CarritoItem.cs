namespace ProgressusWebApi.Models.MerchModels
{
    public class CarritoItem
    {
        public int Id { get; set; } // Clave primaria
        public string ProductoId { get; set; } // Relación con el producto
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario; // Propiedad calculada
        public string CarritoId { get; set; } // Relación con el carrito
        public Carrito Carrito { get; set; } // Propiedad de navegación
    }
}
