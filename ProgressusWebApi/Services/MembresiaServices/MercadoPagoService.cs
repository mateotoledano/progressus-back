using MercadoPago.Client.Preference;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Identity;
using ProgressusWebApi.Dtos.MercadoPagoDtos;
using ProgressusWebApi.Models.MembresiaModels;
using ProgressusWebApi.Models.MerchModels;
using System.Net.Http;
using System.Text.Json;
using WebApiMercadoPago.Repositories.Interface;
using WebApiMercadoPago.Services.Interface;

namespace WebApiMercadoPago.Services
{
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly IMercadoPagoRepository _mercadoPagoRepository;
        private const string MercadoPagoBaseUrl = "https://api.mercadopago.com/v1/payments/";

        public MercadoPagoService(IMercadoPagoRepository mercadoPagoRepository)
        {
            _mercadoPagoRepository = mercadoPagoRepository;
        }

        public async Task<Preference> CreatePreferenceAsync(PreferenceRequest preference)
        {
            SolicitudDePago solicitud = new SolicitudDePago();
            return await _mercadoPagoRepository.CreatePreferenceAsync(solicitud);
        }

        public async Task<Preference> CrearPreferenciaDePagoCarritoAsync(Carrito carrito, string pedidoId)
        {
            return await _mercadoPagoRepository.CrearPreferenciaDeCarritoAsync(carrito, pedidoId);
        }

        public async Task<string> ConsultarEstadoPreferencia(string id)
        {
            try
            {
                var httpClient = new HttpClient();

                // Configurar la solicitud
                var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, $"{MercadoPagoBaseUrl}{id}");
                request.Headers.Add("Authorization", "Bearer APP_USR-2278733141716614-062815-583c9779901a7bbf32c8e8a73971e44c-1878150528");

                // Realizar la solicitud
                HttpResponseMessage response = await httpClient.SendAsync(request);
                var res = response.Content.ReadAsStreamAsync().Result;
                var data = JsonSerializer.Deserialize<PaymentDto>(res, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data.status;
            }
            catch
            {
                // la tiro porque ya está cacheada en el controller
                throw;                
            }

        } 

    }
}
