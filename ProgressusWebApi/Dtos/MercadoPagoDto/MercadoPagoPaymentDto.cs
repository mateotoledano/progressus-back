using System.Text.Json.Serialization;

namespace ProgressusWebApi.Dtos.MercadoPagoDto
{
    public class MercadoPagoPaymentDto
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("date_created")]
        public DateTime date_created { get; set; }

        [JsonPropertyName("date_approved")]
        public DateTime date_approved { get; set; }

        [JsonPropertyName("date_last_updated")]
        public DateTime date_last_updated { get; set; }

        [JsonPropertyName("money_release_date")]
        public DateTime money_release_date { get; set; }

        [JsonPropertyName("payment_method_id")]
        public string payment_method_id { get; set; }

        [JsonPropertyName("payment_type_id")]
        public string payment_type_id { get; set; }

        [JsonPropertyName("status")]
        public string status { get; set; }

        [JsonPropertyName("status_detail")]
        public string status_detail { get; set; }

        [JsonPropertyName("currency_id")]
        public string currency_id { get; set; }

        [JsonPropertyName("description")]
        public string description { get; set; }

        [JsonPropertyName("collector_id")]
        public int collector_id { get; set; }

        [JsonPropertyName("payer")]
        public PayerDto payer { get; set; }

        [JsonPropertyName("metadata")]
        public object metadata { get; set; }

        [JsonPropertyName("additional_info")]
        public object additional_info { get; set; }

        [JsonPropertyName("external_reference")]
        public string external_reference { get; set; }

        [JsonPropertyName("transaction_amount")]
        public decimal transaction_amount { get; set; }

        [JsonPropertyName("transaction_amount_refunded")]
        public decimal transaction_amount_refunded { get; set; }

        [JsonPropertyName("coupon_amount")]
        public decimal coupon_amount { get; set; }

        [JsonPropertyName("transaction_details")]
        public TransactionDetailsDto transaction_details { get; set; }

        [JsonPropertyName("installments")]
        public int installments { get; set; }

        [JsonPropertyName("card")]
        public object card { get; set; }
    }
}
