using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using ProgressusWebApi.DataContext;
using WebApiMercadoPago.Repositories.Interface;

namespace WebApiMercadoPago.Repositories
{
    public class MercadoPagoRepository : IMercadoPagoRepository
    {

        private readonly ProgressusDataContext _context;
        public MercadoPagoRepository(ProgressusDataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<Preference> CreatePreferenceAsync(PreferenceRequest preference)
        {
            var client = new PreferenceClient();

            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                  new PreferenceItemRequest
                  {
                      Id = "item-ID-1234",
                      Title = "Meu produto",
                      CurrencyId = "ARS",
                      PictureUrl = "https://www.mercadopago.com/org-img/MP3/home/logomp3.gif",
                      Description = "Descrição do Item",
                      CategoryId = "art",
                      Quantity = 1,
                      UnitPrice = 75.76m
                  }
                },
                Payer = new PreferencePayerRequest
                {
                    Name = "João",
                    Surname = "Silva",
                    Email = "user@email.com",
                    Phone = new PhoneRequest
                    {
                        AreaCode = "11",
                        Number = "4444-4444"
                    },
                    Identification = new IdentificationRequest
                    {
                        Type = "CPF",
                        Number = "19119119100"
                    },
                    Address = new AddressRequest
                    {
                        StreetName = "Street",
                        StreetNumber = "123",
                        ZipCode = "06233200"
                    }
                },
                
                //Redirección del usuario según resultado del pago
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://www.success.com",
                    Failure = "https://www.failure.com",
                    Pending = "https://www.pending.com"
                },

                //URL para recibir la información del pago        
                NotificationUrl = "https://www.notification.com",

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
    }
}
