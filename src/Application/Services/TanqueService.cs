using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TanqueService : ITanqueService
    {
        private readonly ITanqueRepository _tanqueRepository;

        public TanqueService(ITanqueRepository tanqueRepository)
        {
            _tanqueRepository = tanqueRepository;
        }

        public async Task<IEnumerable<TanqueDto>> GetAllTanquesAsync()
        {
            var tanques = await _tanqueRepository.GetAllAsync();
            return tanques.Select(MapToDto);
        }

        public async Task<TanqueDto?> GetTanqueByIdAsync(int id)
        {
            var tanque = await _tanqueRepository.GetByIdAsync(id);
            return tanque != null ? MapToDto(tanque) : null;
        }

        public async Task<TanqueDto> CreateTanqueAsync(CreateTanqueDto createTanqueDto)
        {
            var tanque = new Tanque
            {
                Nombre = createTanqueDto.Nombre,
                Descripcion = createTanqueDto.Descripcion,
                CapacidadMaxima = createTanqueDto.CapacidadMaxima,
                NivelAgua = createTanqueDto.NivelInicial,
                EstaActivo = true
            };

            var createdTanque = await _tanqueRepository.CreateAsync(tanque);
            return MapToDto(createdTanque);
        }

        public async Task<TanqueDto> UpdateNivelTanqueAsync(int id, UpdateNivelTanqueDto updateDto)
        {
            return await UpdateNivelTanqueAsync(id, updateDto.NivelAgua);
        }

        public async Task<TanqueDto> UpdateNivelTanqueAsync(int id, double nuevoNivel)
        {
            var tanque = await _tanqueRepository.GetByIdAsync(id);
            if (tanque == null)
                throw new KeyNotFoundException($"Tanque with ID {id} not found");

            tanque.ActualizarNivel(nuevoNivel);
            var updatedTanque = await _tanqueRepository.UpdateAsync(tanque);
            return MapToDto(updatedTanque);
        }

        public async Task DeleteTanqueAsync(int id)
        {
            var exists = await _tanqueRepository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException($"Tanque with ID {id} not found");

            await _tanqueRepository.DeleteAsync(id);
        }

        public async Task<TanqueResumenDto> GetResumenTanquesAsync()
        {
            var tanques = await _tanqueRepository.GetAllAsync();
            var tanquesList = tanques.ToList();

            if (!tanquesList.Any())
            {
                return new TanqueResumenDto
                {
                    UltimaActualizacion = DateTime.UtcNow
                };
            }

            return new TanqueResumenDto
            {
                TotalTanques = tanquesList.Count,
                TanquesActivos = tanquesList.Count(t => t.EstaActivo),
                TanquesInactivos = tanquesList.Count(t => !t.EstaActivo),
                NivelPromedioGeneral = tanquesList.Where(t => t.EstaActivo).Average(t => t.NivelAgua),
                CapacidadTotalSistema = tanquesList.Sum(t => t.CapacidadMaxima),
                LitrosTotalesActuales = tanquesList.Sum(t => t.GetLitrosActuales()),
                TanquesNivelCritico = tanquesList.Count(t => t.EstaActivo && t.NivelAgua < 20),
                TanquesNivelBajo = tanquesList.Count(t => t.EstaActivo && t.NivelAgua >= 20 && t.NivelAgua < 50),
                TanquesNivelMedio = tanquesList.Count(t => t.EstaActivo && t.NivelAgua >= 50 && t.NivelAgua < 80),
                TanquesNivelAlto = tanquesList.Count(t => t.EstaActivo && t.NivelAgua >= 80),
                UltimaActualizacion = tanquesList.Max(t => t.UltimaActualizacion)
            };
        }

        public async Task<IEnumerable<TanqueDto>> GetTanquesActivosAsync()
        {
            var tanques = await _tanqueRepository.GetTanquesActivosAsync();
            return tanques.Select(MapToDto);
        }

        public async Task<IEnumerable<TanqueDto>> GetTanquesPorEstadoNivelAsync(string estadoNivel)
        {
            var (nivelMinimo, nivelMaximo) = estadoNivel.ToLower() switch
            {
                "critico" => (0.0, 19.99),
                "bajo" => (20.0, 49.99),
                "medio" => (50.0, 79.99),
                "alto" => (80.0, 100.0),
                _ => throw new ArgumentException($"Estado de nivel no válido: {estadoNivel}")
            };

            var tanques = await _tanqueRepository.GetTanquesPorRangoNivelAsync(nivelMinimo, nivelMaximo);
            return tanques.Select(MapToDto);
        }

        public async Task<IEnumerable<TanqueDto>> GetTanquesNivelCriticoAsync()
        {
            return await GetTanquesPorEstadoNivelAsync("critico");
        }

        public async Task<IEnumerable<TanqueDto>> GetTanquesPorRangoNivelAsync(double nivelMinimo, double nivelMaximo)
        {
            if (nivelMinimo < 0 || nivelMaximo > 100 || nivelMinimo > nivelMaximo)
                throw new ArgumentException("Rango de nivel no válido");

            var tanques = await _tanqueRepository.GetTanquesPorRangoNivelAsync(nivelMinimo, nivelMaximo);
            return tanques.Select(MapToDto);
        }

        public async Task<HistorialNivelDto> RegistrarCambioNivelAsync(int tanqueId, double nuevoNivel)
        {
            var tanque = await _tanqueRepository.GetByIdAsync(tanqueId);
            if (tanque == null)
                throw new KeyNotFoundException($"Tanque with ID {tanqueId} not found");

            var nivelAnterior = tanque.NivelAgua;
            tanque.ActualizarNivel(nuevoNivel);
            await _tanqueRepository.UpdateAsync(tanque);

            var diferencia = nuevoNivel - nivelAnterior;
            var tipoMovimiento = diferencia > 0 ? "Subida" : diferencia < 0 ? "Bajada" : "Sin cambio";

            return new HistorialNivelDto
            {
                TanqueId = tanqueId,
                NombreTanque = tanque.Nombre,
                NivelAnterior = nivelAnterior,
                NivelActual = nuevoNivel,
                DiferenciaNivel = diferencia,
                FechaActualizacion = DateTime.UtcNow,
                TipoMovimiento = tipoMovimiento
            };
        }

        private static TanqueDto MapToDto(Tanque tanque)
        {
            return new TanqueDto
            {
                Id = tanque.Id,
                Nombre = tanque.Nombre,
                Descripcion = tanque.Descripcion,
                NivelAgua = tanque.NivelAgua,
                CapacidadMaxima = tanque.CapacidadMaxima,
                EstaActivo = tanque.EstaActivo,
                FechaCreacion = tanque.FechaCreacion,
                UltimaActualizacion = tanque.UltimaActualizacion,
                LitrosActuales = tanque.GetLitrosActuales(),
                EstadoNivel = tanque.GetEstadoNivel()
            };
        }
    }
}