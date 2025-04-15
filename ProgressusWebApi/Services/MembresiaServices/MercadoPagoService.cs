using System.Net.Http;
using System.Text.Json;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Identity;
using ProgressusWebApi.Dtos.MercadoPagoDtos;
using ProgressusWebApi.Models.MembresiaModels;
using ProgressusWebApi.Models.MerchModels;
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

        public async Task<Preference> CrearPreferenciaDePagoCarritoAsync(
            Carrito carrito,
            string pedidoId
        )
        {
            return await _mercadoPagoRepository.CrearPreferenciaDeCarritoAsync(carrito, pedidoId);
        }

        public async Task<string> ConsultarEstadoPreferencia(string id)
        {
            try
            {
                var httpClient = new HttpClient();

                // Configurar la solicitud
                var request = new HttpRequestMessage(
                    System.Net.Http.HttpMethod.Get,
                    $"{MercadoPagoBaseUrl}{id}"
                );
                request.Headers.Add(
                    "Authorization",
                    "Bearer APP_USR-8561091671920007-062802-878c7cba462bc355cb6143df2a4634f3-1070425524"
                );

                // Realizar la solicitud
                HttpResponseMessage response = await httpClient.SendAsync(request);
                var res = response.Content.ReadAsStreamAsync().Result;
                var data = JsonSerializer.Deserialize<PaymentDto>(
                    res,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

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
