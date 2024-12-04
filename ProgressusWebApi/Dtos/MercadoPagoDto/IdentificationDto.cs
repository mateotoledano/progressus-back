using System.Text.Json.Serialization;

namespace ProgressusWebApi.Dtos.MercadoPagoDto
{
    public class IdentificationDto
    {
        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("number")]
        public long number { get; set; }
    }
}
