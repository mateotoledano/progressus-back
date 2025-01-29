namespace ProgressusWebApi.Models.PlanNutricional
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Objetivo { get; set; }
        public double PorcentajeDeGrasa { get; set; }
        public int Edad { get; set; }
        public double Peso { get; set; }
    }
}
