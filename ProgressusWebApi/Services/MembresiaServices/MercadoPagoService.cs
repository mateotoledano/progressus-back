using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using WebApiMercadoPago.Repositories.Interface;
using WebApiMercadoPago.Services.Interface;

namespace WebApiMercadoPago.Services
{
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly IMercadoPagoRepository _mercadoPagoRepository;

        public MercadoPagoService(IMercadoPagoRepository mercadoPagoRepository)
        {
            _mercadoPagoRepository = mercadoPagoRepository;
        }

        public async Task<Preference> CreatePreferenceAsync(PreferenceRequest preference)
        {
            return await _mercadoPagoRepository.CreatePreferenceAsync(preference);
        }
    }
}
