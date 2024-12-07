using System.Text.Json.Serialization;

namespace ProgressusWebApi.Dtos.MercadoPagoDto
{
    public class TransactionDetailsDto
    {
        [JsonPropertyName("net_received_amount")]
        public decimal net_received_amount { get; set; }

        [JsonPropertyName("total_paid_amount")]
        public decimal total_paid_amount { get; set; }

        [JsonPropertyName("overpaid_amount")]
        public decimal overpaid_amount { get; set; }

        [JsonPropertyName("installment_amount")]
        public decimal installment_amount { get; set; }
    }
}
