using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

// Actualizar la interfaz IBombaService
public interface IBombaServiceExtended : IBombaService
{
    Task<BombaDto> MarcarFallaBombaAsync(int id, TipoFalla tipoFalla);
    Task<BombaDto> RepararBombaAsync(int id);
    Task<BombaDto> PonerEnMantenimientoAsync(int id);
    Task<BombaDto> FinalizarMantenimientoAsync(int id);
    Task<IEnumerable<BombaDto>> GetBombasOperativasAsync();
    Task<IEnumerable<BombaDto>> GetBombasConFallaAsync();
}

public class BombaService : IBombaServiceExtended
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
            FlujometroActivo = false,
            Estado = EstadoBomba.Apagada,
            TipoFalla = TipoFalla.SinFalla,
            EsBombaReserva = false,
            Prioridad = 1
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

        if (!bomba.PuedeEncenderse())
        {
            throw new InvalidOperationException($"La bomba {bomba.Nombre} no puede encenderse. Estado actual: {bomba.Estado}");
        }

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

    public async Task<BombaDto> MarcarFallaBombaAsync(int id, TipoFalla tipoFalla)
    {
        var bomba = await _bombaRepository.GetByIdAsync(id);
        if (bomba == null)
            throw new KeyNotFoundException($"Bomba with ID {id} not found");

        bomba.MarcarFalla(tipoFalla);
        var updatedBomba = await _bombaRepository.UpdateAsync(bomba);
        return MapToDto(updatedBomba);
    }

    public async Task<BombaDto> RepararBombaAsync(int id)
    {
        var bomba = await _bombaRepository.GetByIdAsync(id);
        if (bomba == null)
            throw new KeyNotFoundException($"Bomba with ID {id} not found");

        bomba.RepararFalla();
        var updatedBomba = await _bombaRepository.UpdateAsync(bomba);
        return MapToDto(updatedBomba);
    }

    public async Task<BombaDto> PonerEnMantenimientoAsync(int id)
    {
        var bomba = await _bombaRepository.GetByIdAsync(id);
        if (bomba == null)
            throw new KeyNotFoundException($"Bomba with ID {id} not found");

        bomba.PonerEnMantenimiento();
        var updatedBomba = await _bombaRepository.UpdateAsync(bomba);
        return MapToDto(updatedBomba);
    }

    public async Task<BombaDto> FinalizarMantenimientoAsync(int id)
    {
        var bomba = await _bombaRepository.GetByIdAsync(id);
        if (bomba == null)
            throw new KeyNotFoundException($"Bomba with ID {id} not found");

        bomba.FinalizarMantenimiento();
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

    public async Task<IEnumerable<BombaDto>> GetBombasOperativasAsync()
    {
        var bombas = await _bombaRepository.GetAllAsync();
        return bombas.Where(b => b.EstaOperativa()).Select(MapToDto);
    }

    public async Task<IEnumerable<BombaDto>> GetBombasConFallaAsync()
    {
        var bombas = await _bombaRepository.GetAllAsync();
        return bombas.Where(b => b.Estado == EstadoBomba.Falla).Select(MapToDto);
    }

    public async Task<IEnumerable<BombaDto>> EncenderTodasLasBombasAsync()
    {
        var bombas = await _bombaRepository.GetAllAsync();
        var bombasActualizadas = new List<BombaDto>();

        foreach (var bomba in bombas.Where(b => b.PuedeEncenderse()))
        {
            try
            {
                bomba.Encender();
                var bombaActualizada = await _bombaRepository.UpdateAsync(bomba);
                bombasActualizadas.Add(MapToDto(bombaActualizada));
            }
            catch (InvalidOperationException)
            {
                // Bomba no puede encenderse, continuar con las siguientes
                continue;
            }
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

    // Nuevo MapToDto extendido para incluir todos los campos
    private static BombaDto MapToDtoExtended(Bomba bomba)
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
            Estado = bomba.Estado.ToString(),
            TipoFalla = bomba.TipoFalla.ToString(),
            EsBombaReserva = bomba.EsBombaReserva,
            Prioridad = bomba.Prioridad,
            UltimaFalla = bomba.UltimaFalla,
            HorasOperacion = bomba.HorasOperacion,
            FechaCreacion = bomba.FechaCreacion,
            UltimaActualizacion = bomba.UltimaActualizacion,
            UltimoMantenimiento = bomba.UltimoMantenimiento,
            EstaOperativa = bomba.EstaOperativa(),
            PuedeEncenderse = bomba.PuedeEncenderse()
        };
    }
}