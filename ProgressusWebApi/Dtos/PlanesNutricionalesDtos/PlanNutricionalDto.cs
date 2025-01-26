namespace ProgressusWebApi.Dtos.PlanesNutricionalesDtos
{
    public class PlanNutricionalDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<DiaPlanDto> Dias { get; set; }
    }
}
