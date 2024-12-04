namespace ProgressusWebApi.Dtos.PlanDeEntrenamientoDtos.PlanDeEntrenamientoDto
{
    public class AgregarQuitarUnSoloEjercicioDto
    {
        public int PlanId { get; set; }
        public int DiaDePlan { get; set; }
        public int EjercicioId { get; set; }
        public int Series { get; set; }
        public int Repes { get; set; }
        public int Orden { get; set; }
    }
}
