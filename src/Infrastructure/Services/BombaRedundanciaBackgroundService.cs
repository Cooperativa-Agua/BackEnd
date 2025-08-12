using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class BombaRedundanciaBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BombaRedundanciaBackgroundService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1); // Verificar cada minuto

        public BombaRedundanciaBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<BombaRedundanciaBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de monitoreo de redundancia de bombas iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var redundanciaService = scope.ServiceProvider.GetRequiredService<IBombaRedundanciaService>();

                    var resultado = await redundanciaService.VerificarYMantenerRedundanciaAsync();

                    if (resultado.EstadoCritico)
                    {
                        _logger.LogCritical("SISTEMA CRÍTICO: {Descripcion}", resultado.Descripcion);
                        // Aquí podrías agregar notificaciones adicionales como emails, SMS, etc.
                    }
                    else if (resultado.AccionRequerida)
                    {
                        _logger.LogWarning("Acción de redundancia ejecutada: {Descripcion}", resultado.Descripcion);
                    }

                    // Log de bombas activadas/desactivadas automáticamente
                    foreach (var bombaActivada in resultado.BombasActivadas)
                    {
                        _logger.LogInformation("Bomba activada automáticamente: {Bomba} - {Accion}",
                            bombaActivada.NombreBomba, bombaActivada.Accion);
                    }

                    foreach (var bombaDesactivada in resultado.BombasDesactivadas)
                    {
                        _logger.LogInformation("Bomba desactivada automáticamente: {Bomba} - {Accion}",
                            bombaDesactivada.NombreBomba, bombaDesactivada.Accion);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en el servicio de monitoreo de redundancia");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("Servicio de monitoreo de redundancia de bombas detenido");
        }
    }
}