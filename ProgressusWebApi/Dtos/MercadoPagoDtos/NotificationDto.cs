namespace ProgressusWebApi.Dtos.MercadoPagoDtos
{
    public class NotificationDto
    {
        // Pongo todos nulleables porque si alguno no llega se va a romper el serializador de c#
        public int? id { get; set; }
        public bool? live_mode { get; set; }
        public string? type { get; set; }
        public DateTime? date_created { get; set; }
        public int? user_id { get; set; }
        public string? api_version { get; set; }
        public string? action { get; set; }
        public Data? data { get; set; }
    }

    public class Data
    {
        public string? id { get; set; }
    }

}
