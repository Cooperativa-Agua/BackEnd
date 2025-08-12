using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class TanqueRepository : ITanqueRepository
    {
        private readonly ApplicationDbContext _context;

        public TanqueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tanque>> GetAllAsync()
        {
            return await _context.Tanques.ToListAsync();
        }

        public async Task<Tanque?> GetByIdAsync(int id)
        {
            return await _context.Tanques.FindAsync(id);
        }

        public async Task<Tanque> CreateAsync(Tanque tanque)
        {
            _context.Tanques.Add(tanque);
            await _context.SaveChangesAsync();
            return tanque;
        }

        public async Task<Tanque> UpdateAsync(Tanque tanque)
        {
            _context.Entry(tanque).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return tanque;
        }

        public async Task DeleteAsync(int id)
        {
            var tanque = await _context.Tanques.FindAsync(id);
            if (tanque != null)
            {
                _context.Tanques.Remove(tanque);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Tanques.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tanque>> GetTanquesActivosAsync()
        {
            return await _context.Tanques
                .Where(t => t.EstaActivo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tanque>> GetTanquesPorRangoNivelAsync(double nivelMinimo, double nivelMaximo)
        {
            return await _context.Tanques
                .Where(t => t.EstaActivo && t.NivelAgua >= nivelMinimo && t.NivelAgua <= nivelMaximo)
                .ToListAsync();
        }
    }
}