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
    public class BombaRepository : IBombaRepository
    {
        private readonly ApplicationDbContext _context;

        public BombaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bomba>> GetAllAsync()
        {
            return await _context.Bombas.ToListAsync();
        }

        public async Task<Bomba?> GetByIdAsync(int id)
        {
            return await _context.Bombas.FindAsync(id);
        }

        public async Task<Bomba> CreateAsync(Bomba bomba)
        {
            _context.Bombas.Add(bomba);
            await _context.SaveChangesAsync();
            return bomba;
        }

        public async Task<Bomba> UpdateAsync(Bomba bomba)
        {
            _context.Entry(bomba).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return bomba;
        }

        public async Task DeleteAsync(int id)
        {
            var bomba = await _context.Bombas.FindAsync(id);
            if (bomba != null)
            {
                _context.Bombas.Remove(bomba);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Bombas.AnyAsync(b => b.Id == id);
        }
    }
}
