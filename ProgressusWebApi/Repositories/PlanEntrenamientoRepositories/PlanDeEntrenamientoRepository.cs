using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Model;
using ProgressusWebApi.Repositories.Interfaces;

namespace ProgressusWebApi.Repositories.PlanEntrenamientoRepositories 
{
    public class PlanDeEntrenamientoRepository : IPlanDeEntrenamientoRepository
    {
        private readonly ProgressusDataContext _context;

        public PlanDeEntrenamientoRepository(ProgressusDataContext context)
        {
            _context = context;
        }

        public async Task<PlanDeEntrenamiento> Crear(PlanDeEntrenamiento plan)
        {
            _context.PlanesDeEntrenamiento.Add(plan);
            await _context.SaveChangesAsync();
            return plan;
        }

        public async Task<PlanDeEntrenamiento?> ObtenerPorId(int id)
        {
            return await _context.PlanesDeEntrenamiento
                .Include(p => p.DiasDelPlan)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<PlanDeEntrenamiento>> ObtenerPorNombre(string nombre)
        {
            return await _context.PlanesDeEntrenamiento
                .Where(p => p.Nombre.Contains(nombre))
                .ToListAsync();
        }

        public async Task<List<PlanDeEntrenamiento>> ObtenerPorObjetivo(int objetivoDelPlanId)
        {
            return await _context.PlanesDeEntrenamiento
                .Where(p => p.ObjetivoDelPlanId == objetivoDelPlanId)
                .ToListAsync();
        }

        public async Task<PlanDeEntrenamiento> Actualizar(int id, PlanDeEntrenamiento plan)
        {
            _context.PlanesDeEntrenamiento.Update(plan);
            await _context.SaveChangesAsync();
            return plan;
        }

        public async Task<bool> Eliminar(int id)
        {
            var plan = await _context.PlanesDeEntrenamiento.FindAsync(id);
            if (plan != null)
            {
                _context.PlanesDeEntrenamiento.Remove(plan);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<PlanDeEntrenamiento>> ObtenerPlantillasDePlanes()
        {
            return await _context.PlanesDeEntrenamiento
                .Where(p => p.EsPlantilla == true)
                .ToListAsync();
        }
    }
}
