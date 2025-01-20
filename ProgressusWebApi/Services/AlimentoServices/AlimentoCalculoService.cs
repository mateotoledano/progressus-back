using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Models.AlimentosModels;

namespace ProgressusWebApi.Services.AlimentoServices
{
    public class AlimentoCalculoService
    {
        private readonly ProgressusDataContext _context;

        public AlimentoCalculoService(ProgressusDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Alimento>> GetAllAsync()
        {
            return await _context.Alimento.ToListAsync();
        }

        public async Task<Alimento> GetByIdAsync(int id)
        {
            return await _context.Alimento.FindAsync(id);
        }

        public async Task<Alimento> AddAsync(Alimento alimento)
        {
            _context.Alimento.Add(alimento);
            await _context.SaveChangesAsync();
            return alimento;
        }

        public async Task<Alimento> UpdateAsync(Alimento alimento)
        {
            var existing = await _context.Alimento.FindAsync(alimento.Id);
            if (existing == null) throw new KeyNotFoundException("Alimento no encontrado");

            existing.Nombre = alimento.Nombre;
            existing.Porcion = alimento.Porcion;
            existing.Calorias = alimento.Calorias;
            existing.Carbohidratos = alimento.Carbohidratos;
            existing.Proteinas = alimento.Proteinas;
            existing.Grasas = alimento.Grasas;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var alimento = await _context.Alimento.FindAsync(id);
            if (alimento == null) return false;

            _context.Alimento.Remove(alimento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
