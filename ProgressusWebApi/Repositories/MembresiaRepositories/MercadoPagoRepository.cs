using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Identity;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Models.MembresiaModels;
using ProgressusWebApi.Models.MerchModels;
using ProgressusWebApi.Services.AuthServices.Interfaces;
using WebApiMercadoPago.Repositories.Interface;

namespace WebApiMercadoPago.Repositories
{
    public class MercadoPagoRepository : IMercadoPagoRepository
    {

        private readonly ProgressusDataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public MercadoPagoRepository(ProgressusDataContext dataContext, UserManager<IdentityUser> userManager)
        {
            _context = dataContext;
            _userManager = userManager;


        }

        public async Task<Preference> CreatePreferenceAsync(SolicitudDePago solicitud)
        {
            PreferenceClient client = new PreferenceClient();

            var usuario = _userManager.FindByIdAsync(solicitud.IdentityUserId).Result;

            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                  new PreferenceItemRequest
                  {
                      Title = solicitud.Membresia.Nombre,
                      CurrencyId = "ARS",
                      PictureUrl = "https://www.mercadopago.com/org-img/MP3/home/logomp3.gif",
                      Description = solicitud.Membresia.Descripcion,
                      Quantity = 1,
                      UnitPrice = solicitud.Membresia.Precio
                  }
                },
                Payer = new PreferencePayerRequest
                {
                    Name = usuario.UserName,
                    Email = usuario.Email,
                },

                //Redirección del usuario según resultado del pago
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://pages-mp.vercel.app/success",
                    Failure = "https://pages-mp.vercel.app/failure",
                    Pending = "https://pages-mp.vercel.app/pending"
                },

                //URL para recibir la información del pago        
                NotificationUrl = "https://progressuscenter.azurewebsites.net/api/AAMercadoPago/ObtenerRequestMercadoPago",

                AutoReturn = "approved",
                PaymentMethods = new PreferencePaymentMethodsRequest
                {
                    //
                },

                //Expira en una hora
                Expires = true,
                ExpirationDateFrom = DateTime.Now,
                ExpirationDateTo = DateTime.Now.AddHours(1),
            };

            //Devuelve el init-point (link a donde redirigir al cliente para el pago)
            Preference result = await client.CreateAsync(request);
            return result;
        }


        public async Task<Preference> CrearPreferenciaDeCarritoAsync(Carrito carrito, string pedidoId)
        {
            var items = carrito.Items.Select(item => new PreferenceItemRequest
            {
                Title = item.Merch?.Nombre, 
                Quantity = item.Cantidad,
                CurrencyId = "ARS",
                UnitPrice = item.PrecioUnitario,
                Description = item.Merch?.Nombre                
            }).ToList();

            var request = new PreferenceRequest
            {
                Items = items,
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://pages-mp.vercel.app/success",
                    Failure = "https://pages-mp.vercel.app/failure",
                    Pending = "https://pages-mp.vercel.app/pending"
                },
                NotificationUrl = "https://progressuscenter.azurewebsites.net/api/AAMercadoPago/NotificarPedidoCompletado/" + pedidoId ,
                AutoReturn = "approved",
                
                Expires = true,
                ExpirationDateFrom = DateTime.Now,
                ExpirationDateTo = DateTime.Now.AddHours(1),
            };
            var client = new PreferenceClient();
            return await client.CreateAsync(request);
        }

    }
}
