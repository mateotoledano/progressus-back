namespace ProgressusWebApi.Dtos.MerchDtos
{
    public class MerchDtos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public string Marca { get; set; }
        public int Stock { get; set; }
        public string Talle { get; set; }
        public decimal Precio { get; set; }
        public bool Popular { get; set; }
        public string ImagenUrl { get; set; }

    }
}
