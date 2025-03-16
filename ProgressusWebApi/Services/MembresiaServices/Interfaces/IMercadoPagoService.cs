using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using ProgressusWebApi.Models.MerchModels;

namespace WebApiMercadoPago.Services.Interface
{
    public interface IMercadoPagoService
    {
        Task<Preference> CreatePreferenceAsync(PreferenceRequest preference);

        Task<Preference> CrearPreferenciaDePagoCarritoAsync(Carrito carrito, string pedidoId);

        Task<string> ConsultarEstadoPreferencia(string id);
    }
}
