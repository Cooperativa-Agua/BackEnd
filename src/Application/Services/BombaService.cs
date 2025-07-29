using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class BombaService : IBombaService
{
    private readonly IBombaRepository _bombaRepository;

    public BombaService(IBombaRepository bombaRepository)
    {
        _bombaRepository = bombaRepository;
    }

    public async Task<IEnumerable<BombaDto>> GetAllBombasAsync()
    {
        var bombas = await _bombaRepository.GetAllAsync();
        return bombas.Select(MapToDto);
    }

    public async Task<BombaDto?> GetBombaByIdAsync(int id)
    {
        var bomba = await _bombaRepository.GetByIdAsync(id);
        return bomba != null ? MapToDto(bomba) : null;
    }

    public async Task<BombaDto> CreateBombaAsync(CreateBombaDto createBombaDto)
    {
        var bomba = new Bomba
        {
            Nombre = createBombaDto.Nombre,
            Descripcion = createBombaDto.Descripcion,
            EstaEncendida = false,
            RelayActivo = false,
            SalvaMotorActivo = false,
            FlujometroActivo = false
        };

        var createdBomba = await _bombaRepository.CreateAsync(bomba);
        return MapToDto(createdBomba);
    }

    public async Task<BombaDto> UpdateBombaEstadoAsync(int id, UpdateBombaEstadoDto updateDto)
    {
        var bomba = await _bombaRepository.GetByIdAsync(id);
        if (bomba == null)
            throw new KeyNotFoundException($"Bomba with ID {id} not found");

        bomba.EstaEncendida = updateDto.EstaEncendida;
        bomba.ActualizarEstado(updateDto.RelayActivo, updateDto.SalvaMotorActivo, updateDto.FlujometroActivo);

        var updatedBomba = await _bombaRepository.UpdateAsync(bomba);
        return MapToDto(updatedBomba);
    }

    public async Task<BombaDto> EncenderBombaAsync(int id)
    {
        var bomba = await _bombaRepository.GetByIdAsync(id);
        if (bomba == null)
            throw new KeyNotFoundException($"Bomba with ID {id} not found");

        bomba.Encender();
        var updatedBomba = await _bombaRepository.UpdateAsync(bomba);
        return MapToDto(updatedBomba);
    }

    public async Task<BombaDto> ApagarBombaAsync(int id)
    {
        var bomba = await _bombaRepository.GetByIdAsync(id);
        if (bomba == null)
            throw new KeyNotFoundException($"Bomba with ID {id} not found");

        bomba.Apagar();
        var updatedBomba = await _bombaRepository.UpdateAsync(bomba);
        return MapToDto(updatedBomba);
    }

    public async Task DeleteBombaAsync(int id)
    {
        var exists = await _bombaRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"Bomba with ID {id} not found");

        await _bombaRepository.DeleteAsync(id);
    }

    public async Task<BombaEstadoResumenDto> GetEstadoResumenAsync()
    {
        var bombas = await _bombaRepository.GetAllAsync();
        var bombasList = bombas.ToList();

        return new BombaEstadoResumenDto
        {
            TotalBombas = bombasList.Count,
            BombasEncendidas = bombasList.Count(b => b.EstaEncendida),
            BombasApagadas = bombasList.Count(b => !b.EstaEncendida),
            RelaysActivos = bombasList.Count(b => b.RelayActivo),
            SalvaMotoresActivos = bombasList.Count(b => b.SalvaMotorActivo),
            FlujometrosActivos = bombasList.Count(b => b.FlujometroActivo),
            UltimaActualizacion = bombasList.Any() ? bombasList.Max(b => b.UltimaActualizacion) : DateTime.UtcNow
        };
    }

    public async Task<IEnumerable<BombaDto>> GetBombasEncendidasAsync()
    {
        var bombas = await _bombaRepository.GetAllAsync();
        return bombas.Where(b => b.EstaEncendida).Select(MapToDto);
    }

    public async Task<IEnumerable<BombaDto>> GetBombasApagadasAsync()
    {
        var bombas = await _bombaRepository.GetAllAsync();
        return bombas.Where(b => !b.EstaEncendida).Select(MapToDto);
    }

    public async Task<IEnumerable<BombaDto>> EncenderTodasLasBombasAsync()
    {
        var bombas = await _bombaRepository.GetAllAsync();
        var bombasActualizadas = new List<BombaDto>();

        foreach (var bomba in bombas.Where(b => !b.EstaEncendida))
        {
            bomba.Encender();
            var bombaActualizada = await _bombaRepository.UpdateAsync(bomba);
            bombasActualizadas.Add(MapToDto(bombaActualizada));
        }

        return bombasActualizadas;
    }

    public async Task<IEnumerable<BombaDto>> ApagarTodasLasBombasAsync()
    {
        var bombas = await _bombaRepository.GetAllAsync();
        var bombasActualizadas = new List<BombaDto>();

        foreach (var bomba in bombas.Where(b => b.EstaEncendida))
        {
            bomba.Apagar();
            var bombaActualizada = await _bombaRepository.UpdateAsync(bomba);
            bombasActualizadas.Add(MapToDto(bombaActualizada));
        }

        return bombasActualizadas;
    }

    private static BombaDto MapToDto(Bomba bomba)
    {
        return new BombaDto
        {
            Id = bomba.Id,
            Nombre = bomba.Nombre,
            Descripcion = bomba.Descripcion,
            EstaEncendida = bomba.EstaEncendida,
            RelayActivo = bomba.RelayActivo,
            SalvaMotorActivo = bomba.SalvaMotorActivo,
            FlujometroActivo = bomba.FlujometroActivo,
            FechaCreacion = bomba.FechaCreacion,
            UltimaActualizacion = bomba.UltimaActualizacion
        };
    }
}