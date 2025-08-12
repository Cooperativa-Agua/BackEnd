using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITanqueService
    {
        Task<IEnumerable<TanqueDto>> GetAllTanquesAsync();
        Task<TanqueDto?> GetTanqueByIdAsync(int id);
        Task<TanqueDto> CreateTanqueAsync(CreateTanqueDto createTanqueDto);
        Task<TanqueDto> UpdateNivelTanqueAsync(int id, UpdateNivelTanqueDto updateDto);
        Task<TanqueDto> UpdateNivelTanqueAsync(int id, double nuevoNivel);
        Task DeleteTanqueAsync(int id);
        Task<TanqueResumenDto> GetResumenTanquesAsync();
        Task<IEnumerable<TanqueDto>> GetTanquesActivosAsync();
        Task<IEnumerable<TanqueDto>> GetTanquesPorEstadoNivelAsync(string estadoNivel);
        Task<IEnumerable<TanqueDto>> GetTanquesNivelCriticoAsync();
        Task<IEnumerable<TanqueDto>> GetTanquesPorRangoNivelAsync(double nivelMinimo, double nivelMaximo);
        Task<HistorialNivelDto> RegistrarCambioNivelAsync(int tanqueId, double nuevoNivel);
    }
}