using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using ProgressusWebApi.Models.MembresiaModels;
using ProgressusWebApi.Models.MerchModels;

namespace WebApiMercadoPago.Repositories.Interface
{
    public interface IMercadoPagoRepository
    {
        Task<Preference> CreatePreferenceAsync(SolicitudDePago solicitud);

        Task<Preference> CrearPreferenciaDeCarritoAsync(Carrito carrito, string pedidoId);
    }
}
