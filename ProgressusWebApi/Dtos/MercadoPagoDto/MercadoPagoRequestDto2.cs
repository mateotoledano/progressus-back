namespace ProgressusWebApi.Dtos.MercadoPagoDto
{
    public class MercadoPagoRequestDto2
    {
        public long id { get; set; }
        public bool live_mode { get; set; }
        public string type { get; set; }
        public DateTime date_created { get; set; }
        public long user_id { get; set; }
        public string api_version { get; set; }
        public string action { get; set; }
        public NotificationData data { get; set; }
    }

    public class NotificationData
    {
        public string id { get; set; }
    }
}
