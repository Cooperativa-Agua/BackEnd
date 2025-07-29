using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IBombaRepository
{
    Task<IEnumerable<Bomba>> GetAllAsync();
    Task<Bomba?> GetByIdAsync(int id);
    Task<Bomba> CreateAsync(Bomba bomba);
    Task<Bomba> UpdateAsync(Bomba bomba);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

