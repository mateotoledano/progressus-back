﻿using ProgressusWebApi.Model;

namespace ProgressusWebApi.Repositories.Interfaces
{
    public interface IPlanDeEntrenamientoRepository
    {
        Task<PlanDeEntrenamiento> Crear(PlanDeEntrenamiento planDeEntrenamiento);
        Task<bool> Eliminar(int id);
        Task<PlanDeEntrenamiento> ObtenerPorId(int id);
        Task<List<PlanDeEntrenamiento>> ObtenerPorNombre(string nombre);
        Task<PlanDeEntrenamiento?> Actualizar(int id, PlanDeEntrenamiento planDeEntrenamiento);
        Task<List<PlanDeEntrenamiento>> ObtenerPorObjetivo(int objetivoDePlanId);
        Task<List<PlanDeEntrenamiento>> ObtenerPlantillasDePlanes();
    }
}