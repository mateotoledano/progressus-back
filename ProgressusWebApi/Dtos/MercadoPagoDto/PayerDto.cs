using System.Text.Json.Serialization;

namespace ProgressusWebApi.Dtos.MercadoPagoDto
{
    public class PayerDto
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("email")]
        public string email { get; set; }

        [JsonPropertyName("identification")]
        public IdentificationDto identification { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }
    }
}
