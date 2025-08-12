using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITanqueRepository
    {
        Task<IEnumerable<Tanque>> GetAllAsync();
        Task<Tanque?> GetByIdAsync(int id);
        Task<Tanque> CreateAsync(Tanque tanque);
        Task<Tanque> UpdateAsync(Tanque tanque);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Tanque>> GetTanquesActivosAsync();
        Task<IEnumerable<Tanque>> GetTanquesPorRangoNivelAsync(double nivelMinimo, double nivelMaximo);
    }
}