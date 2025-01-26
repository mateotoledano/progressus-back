namespace ProgressusWebApi.Dtos.PlanesNutricionalesDtos
{
    public class ComidaDto
    {
        public string TipoComida { get; set; }
        public List<AlimentoComidaDto> Alimentos { get; set; }
    }
}
