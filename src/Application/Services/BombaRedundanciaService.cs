using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
  

    public class BombaRedundanciaService : IBombaRedundanciaService
    {
        private readonly IBombaRepository _bombaRepository;
        private readonly ILogger<BombaRedundanciaService> _logger;
        private const int BOMBAS_REQUERIDAS_ACTIVAS = 2;

        public BombaRedundanciaService(IBombaRepository bombaRepository, ILogger<BombaRedundanciaService> logger)
        {
            _bombaRepository = bombaRepository;
            _logger = logger;
        }

        public async Task<RedundanciaResultDto> VerificarYMantenerRedundanciaAsync()
        {
            var bombas = await _bombaRepository.GetAllAsync();
            var bombasList = bombas.ToList();

            var bombasEncendidas = bombasList.Where(b => b.EstaEncendida && b.EstaOperativa()).ToList();
            var bombasOperativas = bombasList.Where(b => b.EstaOperativa()).ToList();

            var resultado = new RedundanciaResultDto
            {
                FechaVerificacion = DateTime.UtcNow,
                BombasRequeridas = BOMBAS_REQUERIDAS_ACTIVAS,
                BombasActivas = bombasEncendidas.Count,
                BombasDisponibles = bombasOperativas.Count
            };

            _logger.LogInformation("Verificando redundancia - Activas: {Activas}, Disponibles: {Disponibles}",
                bombasEncendidas.Count, bombasOperativas.Count);

            if (bombasEncendidas.Count < BOMBAS_REQUERIDAS_ACTIVAS)
            {
                resultado.AccionRequerida = true;
                resultado.Descripcion = $"Bombas insuficientes activas ({bombasEncendidas.Count}/{BOMBAS_REQUERIDAS_ACTIVAS})";

                // Intentar activar bombas de reserva
                var bombasParaActivar = bombasOperativas
                    .Where(b => !b.EstaEncendida)
                    .OrderBy(b => b.Prioridad)
                    .ThenBy(b => b.HorasOperacion) // Priorizar bombas con menos horas de uso
                    .Take(BOMBAS_REQUERIDAS_ACTIVAS - bombasEncendidas.Count);

                foreach (var bomba in bombasParaActivar)
                {
                    try
                    {
                        bomba.Encender();
                        await _bombaRepository.UpdateAsync(bomba);

                        resultado.BombasActivadas.Add(new BombaAccionDto
                        {
                            BombaId = bomba.Id,
                            NombreBomba = bomba.Nombre,
                            Accion = "Activada automáticamente",
                            Timestamp = DateTime.UtcNow
                        });

                        _logger.LogWarning("Bomba {BombaId} ({Nombre}) activada automáticamente por redundancia",
                            bomba.Id, bomba.Nombre);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al activar bomba {BombaId} automáticamente", bomba.Id);
                    }
                }

                // Verificar si se logró mantener la redundancia
                var bombasAhoraActivas = bombasList.Count(b => b.EstaEncendida && b.EstaOperativa());
                if (bombasAhoraActivas < BOMBAS_REQUERIDAS_ACTIVAS)
                {
                    resultado.EstadoCritico = true;
                    resultado.Descripcion += $" - CRÍTICO: Solo {bombasAhoraActivas} bomba(s) operativa(s)";
                    _logger.LogCritical("ESTADO CRÍTICO: Solo {BombasActivas} bomba(s) operativa(s) de {BombasRequeridas} requeridas",
                        bombasAhoraActivas, BOMBAS_REQUERIDAS_ACTIVAS);
                }
            }
            else if (bombasEncendidas.Count > BOMBAS_REQUERIDAS_ACTIVAS)
            {
                // Optimizar: apagar bombas extra, priorizando las de mayor desgaste
                var bombasExtra = bombasEncendidas
                    .OrderByDescending(b => b.HorasOperacion)
                    .Take(bombasEncendidas.Count - BOMBAS_REQUERIDAS_ACTIVAS);

                foreach (var bomba in bombasExtra)
                {
                    bomba.PonerEnEspera();
                    await _bombaRepository.UpdateAsync(bomba);

                    resultado.BombasDesactivadas.Add(new BombaAccionDto
                    {
                        BombaId = bomba.Id,
                        NombreBomba = bomba.Nombre,
                        Accion = "Puesta en espera (optimización)",
                        Timestamp = DateTime.UtcNow
                    });

                    _logger.LogInformation("Bomba {BombaId} ({Nombre}) puesta en espera para optimización",
                        bomba.Id, bomba.Nombre);
                }
            }
            else
            {
                resultado.Descripcion = "Sistema operando normalmente";
                _logger.LogInformation("Sistema de bombas operando normalmente con {BombasActivas} bombas activas",
                    bombasEncendidas.Count);
            }

            return resultado;
        }

        public async Task<RedundanciaResultDto> ReportarFallaBombaAsync(int bombaId, TipoFalla tipoFalla)
        {
            var bomba = await _bombaRepository.GetByIdAsync(bombaId);
            if (bomba == null)
                throw new KeyNotFoundException($"Bomba with ID {bombaId} not found");

            bomba.MarcarFalla(tipoFalla);
            await _bombaRepository.UpdateAsync(bomba);

            _logger.LogError("Falla reportada en bomba {BombaId} ({Nombre}): {TipoFalla}",
                bombaId, bomba.Nombre, tipoFalla);

            var resultado = new RedundanciaResultDto
            {
                FechaVerificacion = DateTime.UtcNow,
                AccionRequerida = true,
                Descripcion = $"Falla detectada en bomba {bomba.Nombre}: {tipoFalla}"
            };

            resultado.BombasConFalla.Add(new BombaAccionDto
            {
                BombaId = bombaId,
                NombreBomba = bomba.Nombre,
                Accion = $"Falla: {tipoFalla}",
                Timestamp = DateTime.UtcNow
            });

            // Verificar y mantener redundancia después de la falla
            var verificacionResult = await VerificarYMantenerRedundanciaAsync();

            // Combinar resultados
            resultado.BombasActivadas.AddRange(verificacionResult.BombasActivadas);
            resultado.BombasDesactivadas.AddRange(verificacionResult.BombasDesactivadas);
            resultado.EstadoCritico = verificacionResult.EstadoCritico;
            resultado.BombasActivas = verificacionResult.BombasActivas;
            resultado.BombasDisponibles = verificacionResult.BombasDisponibles;

            if (verificacionResult.EstadoCritico)
            {
                resultado.Descripcion += " - " + verificacionResult.Descripcion;
            }

            return resultado;
        }

        public async Task<RedundanciaResultDto> RepararBombaAsync(int bombaId)
        {
            var bomba = await _bombaRepository.GetByIdAsync(bombaId);
            if (bomba == null)
                throw new KeyNotFoundException($"Bomba with ID {bombaId} not found");

            bomba.RepararFalla();
            await _bombaRepository.UpdateAsync(bomba);

            _logger.LogInformation("Bomba {BombaId} ({Nombre}) reparada y disponible nuevamente",
                bombaId, bomba.Nombre);

            var resultado = new RedundanciaResultDto
            {
                FechaVerificacion = DateTime.UtcNow,
                Descripcion = $"Bomba {bomba.Nombre} reparada exitosamente"
            };

            // Verificar si necesita activarse para mantener redundancia
            var verificacionResult = await VerificarYMantenerRedundanciaAsync();

            resultado.BombasActivadas.AddRange(verificacionResult.BombasActivadas);
            resultado.BombasActivas = verificacionResult.BombasActivas;
            resultado.BombasDisponibles = verificacionResult.BombasDisponibles;

            return resultado;
        }

        public async Task<EstadoRedundanciaDto> GetEstadoRedundanciaAsync()
        {
            var bombas = await _bombaRepository.GetAllAsync();
            var bombasList = bombas.ToList();

            var bombasEncendidas = bombasList.Where(b => b.EstaEncendida).ToList();
            var bombasOperativas = bombasList.Where(b => b.EstaOperativa()).ToList();
            var bombasConFalla = bombasList.Where(b => b.Estado == EstadoBomba.Falla).ToList();
            var bombasEnMantenimiento = bombasList.Where(b => b.Estado == EstadoBomba.Mantenimiento).ToList();

            return new EstadoRedundanciaDto
            {
                TotalBombas = bombasList.Count,
                BombasRequeridas = BOMBAS_REQUERIDAS_ACTIVAS,
                BombasActivas = bombasEncendidas.Count,
                BombasOperativas = bombasOperativas.Count,
                BombasConFalla = bombasConFalla.Count,
                BombasEnMantenimiento = bombasEnMantenimiento.Count,
                EstadoSistema = DeterminarEstadoSistema(bombasEncendidas.Count, bombasOperativas.Count),
                UltimaVerificacion = DateTime.UtcNow,
                Bombas = bombasList.Select(b => new BombaEstadoDto
                {
                    Id = b.Id,
                    Nombre = b.Nombre,
                    Estado = b.Estado.ToString(),
                    EstaEncendida = b.EstaEncendida,
                    TipoFalla = b.TipoFalla.ToString(),
                    Prioridad = b.Prioridad,
                    HorasOperacion = b.HorasOperacion,
                    UltimaActualizacion = b.UltimaActualizacion
                }).ToList()
            };
        }

        public async Task<RedundanciaResultDto> ForzarCambioTurnoAsync()
        {
            var bombas = await _bombaRepository.GetAllAsync();
            var bombasEncendidas = bombas.Where(b => b.EstaEncendida && b.EstaOperativa()).ToList();
            var bombasEnEspera = bombas.Where(b => b.Estado == EstadoBomba.EnEspera || b.Estado == EstadoBomba.Apagada)
                                       .Where(b => b.EstaOperativa())
                                       .OrderBy(b => b.HorasOperacion)
                                       .Take(BOMBAS_REQUERIDAS_ACTIVAS)
                                       .ToList();

            var resultado = new RedundanciaResultDto
            {
                FechaVerificacion = DateTime.UtcNow,
                Descripcion = "Cambio de turno forzado"
            };

            // Apagar bombas actuales
            foreach (var bomba in bombasEncendidas)
            {
                bomba.PonerEnEspera();
                await _bombaRepository.UpdateAsync(bomba);

                resultado.BombasDesactivadas.Add(new BombaAccionDto
                {
                    BombaId = bomba.Id,
                    NombreBomba = bomba.Nombre,
                    Accion = "Desactivada por cambio de turno",
                    Timestamp = DateTime.UtcNow
                });
            }

            // Encender bombas de relevo
            foreach (var bomba in bombasEnEspera)
            {
                bomba.Encender();
                await _bombaRepository.UpdateAsync(bomba);

                resultado.BombasActivadas.Add(new BombaAccionDto
                {
                    BombaId = bomba.Id,
                    NombreBomba = bomba.Nombre,
                    Accion = "Activada por cambio de turno",
                    Timestamp = DateTime.UtcNow
                });
            }

            _logger.LogInformation("Cambio de turno completado - {BombasActivadas} bombas activadas",
                resultado.BombasActivadas.Count);

            return resultado;
        }

        private string DeterminarEstadoSistema(int bombasActivas, int bombasOperativas)
        {
            if (bombasActivas >= BOMBAS_REQUERIDAS_ACTIVAS)
                return "Normal";
            else if (bombasOperativas >= BOMBAS_REQUERIDAS_ACTIVAS)
                return "Advertencia";
            else
                return "Crítico";
        }
    }
}