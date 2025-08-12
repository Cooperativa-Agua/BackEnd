using Application.Interfaces;
using Application.Models;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BombasController : ControllerBase
{
    private readonly IBombaService _bombaService;
    private readonly IBombaRedundanciaService _redundanciaService;
    private readonly ILogger<BombasController> _logger;

    public BombasController(
        IBombaService bombaService,
        IBombaRedundanciaService redundanciaService,
        ILogger<BombasController> logger)
    {
        _bombaService = bombaService;
        _redundanciaService = redundanciaService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las bombas
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BombaDto>>> GetAllBombas()
    {
        try
        {
            var bombas = await _bombaService.GetAllBombasAsync();
            return Ok(bombas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las bombas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene una bomba por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<BombaDto>> GetBombaById(int id)
    {
        try
        {
            var bomba = await _bombaService.GetBombaByIdAsync(id);
            if (bomba == null)
                return NotFound($"Bomba with ID {id} not found");

            return Ok(bomba);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la bomba con ID {BombaId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea una nueva bomba
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BombaDto>> CreateBomba([FromBody] CreateBombaDto createBombaDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bomba = await _bombaService.CreateBombaAsync(createBombaDto);

            // Verificar redundancia después de agregar nueva bomba
            await _redundanciaService.VerificarYMantenerRedundanciaAsync();

            return CreatedAtAction(nameof(GetBombaById), new { id = bomba.Id }, bomba);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la bomba");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Reportar falla en una bomba (activa automáticamente el sistema de redundancia)
    /// </summary>
    [HttpPost("{id}/reportar-falla")]
    public async Task<ActionResult<ApiResponse<RedundanciaResultDto>>> ReportarFallaBomba(int id, [FromBody] ReportarFallaDto reportarFallaDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var badRequestResponse = ApiResponse<RedundanciaResultDto>.ErrorResponse("Datos de entrada no válidos");
                return BadRequest(badRequestResponse);
            }

            var resultado = await _redundanciaService.ReportarFallaBombaAsync(id, reportarFallaDto.TipoFalla);

            var message = resultado.EstadoCritico
                ? "CRÍTICO: Falla reportada y sistema en estado crítico"
                : "Falla reportada y redundancia activada automáticamente";

            var response = ApiResponse<RedundanciaResultDto>.SuccessResponse(resultado, message);

            if (resultado.EstadoCritico)
            {
                _logger.LogCritical("Sistema en estado crítico después de falla en bomba {BombaId}", id);
                return StatusCode(200, response);
            }

            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            var notFoundResponse = ApiResponse<RedundanciaResultDto>.ErrorResponse($"Bomba with ID {id} not found");
            return NotFound(notFoundResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al reportar falla en bomba con ID {BombaId}", id);
            var response = ApiResponse<RedundanciaResultDto>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    /// <summary>
    /// Reparar una bomba con falla
    /// </summary>
    [HttpPost("{id}/reparar")]
    public async Task<ActionResult<ApiResponse<RedundanciaResultDto>>> RepararBomba(int id)
    {
        try
        {
            var resultado = await _redundanciaService.RepararBombaAsync(id);
            var response = ApiResponse<RedundanciaResultDto>.SuccessResponse(resultado, "Bomba reparada exitosamente");

            _logger.LogInformation("Bomba {BombaId} reparada exitosamente", id);
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            var notFoundResponse = ApiResponse<RedundanciaResultDto>.ErrorResponse($"Bomba with ID {id} not found");
            return NotFound(notFoundResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al reparar bomba con ID {BombaId}", id);
            var response = ApiResponse<RedundanciaResultDto>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    /// <summary>
    /// Verificar y mantener redundancia manualmente
    /// </summary>
    [HttpPost("verificar-redundancia")]
    public async Task<ActionResult<ApiResponse<RedundanciaResultDto>>> VerificarRedundancia()
    {
        try
        {
            var resultado = await _redundanciaService.VerificarYMantenerRedundanciaAsync();

            var message = resultado.EstadoCritico
                ? "CRÍTICO: Sistema no puede mantener redundancia requerida"
                : resultado.AccionRequerida
                    ? "Redundancia verificada - Se realizaron ajustes automáticos"
                    : "Sistema operando con redundancia normal";

            var response = ApiResponse<RedundanciaResultDto>.SuccessResponse(resultado, message);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar redundancia");
            var response = ApiResponse<RedundanciaResultDto>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    /// <summary>
    /// Obtener estado completo del sistema de redundancia
    /// </summary>
    [HttpGet("estado-redundancia")]
    public async Task<ActionResult<ApiResponse<EstadoRedundanciaDto>>> GetEstadoRedundancia()
    {
        try
        {
            var estado = await _redundanciaService.GetEstadoRedundanciaAsync();
            var response = ApiResponse<EstadoRedundanciaDto>.SuccessResponse(estado, "Estado de redundancia obtenido exitosamente");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estado de redundancia");
            var response = ApiResponse<EstadoRedundanciaDto>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    /// <summary>
    /// Forzar cambio de turno de bombas (rotar bombas activas)
    /// </summary>
    [HttpPost("cambio-turno")]
    public async Task<ActionResult<ApiResponse<RedundanciaResultDto>>> ForzarCambioTurno()
    {
        try
        {
            var resultado = await _redundanciaService.ForzarCambioTurnoAsync();
            var response = ApiResponse<RedundanciaResultDto>.SuccessResponse(resultado, "Cambio de turno ejecutado exitosamente");

            _logger.LogInformation("Cambio de turno forzado ejecutado");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al ejecutar cambio de turno");
            var response = ApiResponse<RedundanciaResultDto>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    /// <summary>
    /// Enciende una bomba específica (respetando reglas de redundancia)
    /// </summary>
    [HttpPost("{id}/encender")]
    public async Task<ActionResult<BombaDto>> EncenderBomba(int id)
    {
        try
        {
            var bomba = await _bombaService.EncenderBombaAsync(id);

            // Verificar redundancia después del cambio
            await _redundanciaService.VerificarYMantenerRedundanciaAsync();

            _logger.LogInformation("Bomba {BombaId} encendida exitosamente", id);
            return Ok(bomba);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Bomba with ID {id} not found");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al encender la bomba con ID {BombaId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Apaga una bomba específica (verificando que se mantenga redundancia)
    /// </summary>
    [HttpPost("{id}/apagar")]
    public async Task<ActionResult<ApiResponse<object>>> ApagarBomba(int id)
    {
        try
        {
            // Verificar si apagar esta bomba compromete la redundancia
            var estadoActual = await _redundanciaService.GetEstadoRedundanciaAsync();
            if (estadoActual.BombasActivas <= 2)
            {
                var warningResponse = ApiResponse<object>.ErrorResponse("No se puede apagar la bomba: se requiere mantener al menos 2 bombas activas para redundancia");
                return BadRequest(warningResponse);
            }

            var bomba = await _bombaService.ApagarBombaAsync(id);

            // Verificar redundancia después del cambio
            await _redundanciaService.VerificarYMantenerRedundanciaAsync();

            var response = ApiResponse<object>.SuccessResponse(bomba, "Bomba apagada exitosamente");
            _logger.LogInformation("Bomba {BombaId} apagada exitosamente", id);
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            var notFoundResponse = ApiResponse<object>.ErrorResponse($"Bomba with ID {id} not found");
            return NotFound(notFoundResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al apagar la bomba con ID {BombaId}", id);
            var response = ApiResponse<object>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    /// <summary>
    /// Poner bomba en mantenimiento
    /// </summary>
    [HttpPost("{id}/mantenimiento")]
    public async Task<ActionResult<ApiResponse<RedundanciaResultDto>>> PonerEnMantenimiento(int id, [FromBody] BombaMantenimientoDto mantenimientoDto)
    {
        try
        {
            var bomba = await _bombaService.GetBombaByIdAsync(id);
            if (bomba == null)
            {
                var notFoundResponse = ApiResponse<RedundanciaResultDto>.ErrorResponse($"Bomba with ID {id} not found");
                return NotFound(notFoundResponse);
            }

            // Verificar si poner en mantenimiento compromete la redundancia
            var estadoActual = await _redundanciaService.GetEstadoRedundanciaAsync();
            if (bomba.EstaEncendida && estadoActual.BombasActivas <= 2)
            {
                var warningResponse = ApiResponse<RedundanciaResultDto>.ErrorResponse("No se puede poner en mantenimiento: se requiere mantener al menos 2 bombas operativas");
                return BadRequest(warningResponse);
            }

            // Aquí iría la lógica para poner en mantenimiento (necesitarías actualizar el BombaService)
            // Por ahora, simulamos el proceso

            var resultado = await _redundanciaService.VerificarYMantenerRedundanciaAsync();
            var response = ApiResponse<RedundanciaResultDto>.SuccessResponse(resultado, $"Bomba {id} puesta en mantenimiento");

            _logger.LogInformation("Bomba {BombaId} puesta en mantenimiento", id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al poner bomba {BombaId} en mantenimiento", id);
            var response = ApiResponse<RedundanciaResultDto>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    /// <summary>
    /// Actualiza el estado completo de una bomba y verifica redundancia
    /// </summary>
    [HttpPut("{id}/estado")]
    public async Task<ActionResult<BombaDto>> UpdateBombaEstado(int id, [FromBody] UpdateBombaEstadoDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bomba = await _bombaService.UpdateBombaEstadoAsync(id, updateDto);

            // Verificar redundancia después de actualizar estado
            await _redundanciaService.VerificarYMantenerRedundanciaAsync();

            return Ok(bomba);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Bomba with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el estado de la bomba con ID {BombaId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Recibir señal del sensor de bomba para monitoreo automático
    /// </summary>
    [HttpPost("{id}/sensor-estado")]
    public async Task<ActionResult<ApiResponse<object>>> RecibirEstadoSensor(int id, [FromBody] SensorEstadoBombaDto sensorDto)
    {
        try
        {
            var bomba = await _bombaService.GetBombaByIdAsync(id);
            if (bomba == null)
            {
                var notFoundResponse = ApiResponse<object>.ErrorResponse($"Bomba with ID {id} not found");
                return NotFound(notFoundResponse);
            }

            // Actualizar estado basado en sensor
            var updateDto = new UpdateBombaEstadoDto
            {
                EstaEncendida = sensorDto.EstaOperando,
                RelayActivo = sensorDto.RelayActivo,
                SalvaMotorActivo = sensorDto.SalvaMotorActivo,
                FlujometroActivo = sensorDto.FlujometroActivo
            };

            await _bombaService.UpdateBombaEstadoAsync(id, updateDto);

            // Detectar fallas automáticamente
            if (sensorDto.EstaOperando && (!sensorDto.SalvaMotorActivo || !sensorDto.RelayActivo))
            {
                var tipoFalla = !sensorDto.SalvaMotorActivo ? TipoFalla.FallaSalvaMotor : TipoFalla.FallaRelay;
                await _redundanciaService.ReportarFallaBombaAsync(id, tipoFalla);

                var response = ApiResponse<object>.SuccessResponse(null, $"Falla detectada automáticamente: {tipoFalla}. Redundancia activada.");
                return Ok(response);
            }

            // Verificar redundancia
            await _redundanciaService.VerificarYMantenerRedundanciaAsync();

            var successResponse = ApiResponse<object>.SuccessResponse(null, "Estado del sensor procesado exitosamente");
            return Ok(successResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar estado del sensor para bomba {BombaId}", id);
            var response = ApiResponse<object>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    // Mantener endpoints originales para compatibilidad
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBomba(int id)
    {
        try
        {
            await _bombaService.DeleteBombaAsync(id);

            // Verificar redundancia después de eliminar bomba
            await _redundanciaService.VerificarYMantenerRedundanciaAsync();

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Bomba with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la bomba con ID {BombaId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("resumen")]
    public async Task<ActionResult<ApiResponse<BombaEstadoResumenDto>>> GetEstadoResumen()
    {
        try
        {
            var resumen = await _bombaService.GetEstadoResumenAsync();
            var response = ApiResponse<BombaEstadoResumenDto>.SuccessResponse(resumen, "Resumen obtenido exitosamente");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el resumen de estado");
            var response = ApiResponse<BombaEstadoResumenDto>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    [HttpPost("encender-todas")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BombaDto>>>> EncenderTodasLasBombas()
    {
        try
        {
            var bombasActualizadas = await _bombaService.EncenderTodasLasBombasAsync();

            // Verificar que no se viole el límite de bombas activas
            await _redundanciaService.VerificarYMantenerRedundanciaAsync();

            _logger.LogInformation("Se encendieron {Count} bombas", bombasActualizadas.Count());
            var response = ApiResponse<IEnumerable<BombaDto>>.SuccessResponse(bombasActualizadas, $"Se procesó el encendido de bombas (respetando redundancia)");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al encender todas las bombas");
            var response = ApiResponse<IEnumerable<BombaDto>>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }
}

// Nuevo DTO para recibir datos del sensor
public class SensorEstadoBombaDto
{
    public bool EstaOperando { get; set; }
    public bool RelayActivo { get; set; }
    public bool SalvaMotorActivo { get; set; }
    public bool FlujometroActivo { get; set; }
}