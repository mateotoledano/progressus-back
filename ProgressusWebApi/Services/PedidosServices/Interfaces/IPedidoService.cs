using ProgressusWebApi.Dtos.MerchDtos;
using ProgressusWebApi.Models.MerchModels;

namespace ProgressusWebApi.Services.PedidosServices.Interfaces
{
    public interface IPedidoService
    {
        Task<Pedido> RegistrarPedidoAsync(string usuarioId, List<ItemCarritoDto> carrito);

        Task<bool> RegistrarPago(string pedidoId);
    }
}
