using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;

namespace WebApiMercadoPago.Repositories.Interface
{
    public interface IMercadoPagoRepository
    {
        Task<Preference> CreatePreferenceAsync(PreferenceRequest preference);
       
    }
}
