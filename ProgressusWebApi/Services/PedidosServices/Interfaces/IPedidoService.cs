using ProgressusWebApi.Models.MerchModels;

namespace ProgressusWebApi.Services.PedidosServices.Interfaces
{
    public interface IPedidoService
    {
        Task<Pedido> RegistrarPedidoAsync(string usuarioId, Carrito carrito);
    }
}
