using ProgressusWebApi.Models.MerchModels;

namespace ProgressusWebApi.Services.PedidosServices
{
    public class PedidoService
    {
        private static List<Pedido> _pedidos = new List<Pedido>();

        public async Task<Pedido> RegistrarPedidoAsync(string usuarioId, Carrito carrito)
        {
            var pedido = new Pedido
            {
                Id = Guid.NewGuid().ToString(),
                UsuarioId = usuarioId,
                Items = carrito.Items,
                Total = carrito.Total,
                Estado = "Pendiente"
            };

            _pedidos.Add(pedido);
            return pedido;
        }
    }
}

