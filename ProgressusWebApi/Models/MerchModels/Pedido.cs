namespace ProgressusWebApi.Models.MerchModels
{
    public class Pedido
    {
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public List<CarritoItem> Items { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } // "Pendiente", "Pagado", "Cancelado"
    }
}
