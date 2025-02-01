using ProgressusWebApi.Models.MerchModels;

namespace ProgressusWebApi.Services.CarritoServices
{
    public interface ICarritoService
    {
        Task<Carrito> ObtenerCarritoAsync(string usuarioId);
        Task AgregarItemAlCarritoAsync(string usuarioId, CarritoItem item);
        Task EliminarItemDelCarritoAsync(string usuarioId, string productoId);
        Task VaciarCarritoAsync(string usuarioId);

    }
}
