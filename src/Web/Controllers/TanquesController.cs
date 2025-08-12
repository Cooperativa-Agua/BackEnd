using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TanquesController : ControllerBase
    {
        private readonly ITanqueService _tanqueService;
        private readonly ILogger<TanquesController> _logger;

        public TanquesController(ITanqueService tanqueService, ILogger<TanquesController> logger)
        {
            _tanqueService = tanqueService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los tanques
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TanqueDto>>>> GetAllTanques()
        {
            try
            {
                var tanques = await _tanqueService.GetAllTanquesAsync();
                var response = ApiResponse<IEnumerable<TanqueDto>>.SuccessResponse(tanques, "Tanques obtenidos exitosamente");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los tanques");
                var response = ApiResponse<IEnumerable<TanqueDto>>.ErrorResponse("Error interno del servidor");
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Obtiene un tanque por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TanqueDto>>> GetTanqueById(int id)
        {
            try
            {
                var tanque = await _tanqueService.GetTanqueByIdAsync(id);
                if (tanque == null)
                {
                    var notFoundResponse = ApiResponse<TanqueDto>.ErrorResponse($"Tanque with ID {id} not found");
                    return NotFound(notFoundResponse);
                }

                var response = ApiResponse<TanqueDto>.SuccessResponse(tanque, "Tanque obtenido exitosamente");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el tanque con ID {TanqueId}", id);
                var response = ApiResponse<TanqueDto>.ErrorResponse("Error interno del servidor");
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Crea un nuevo tanque
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TanqueDto>>> CreateTanque([FromBody] CreateTanqueDto createTanqueDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var badRequestResponse = ApiResponse<TanqueDto>.ErrorResponse("Datos de entrada no válidos");
                    return BadRequest(badRequestResponse);
                }

                var tanque = await _tanqueService.CreateTanqueAsync(createTanqueDto);
                var response = ApiResponse<TanqueDto>.SuccessResponse(tanque, "Tanque creado exitosamente");
                return CreatedAtAction(nameof(GetTanqueById), new { id = tanque.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el tanque");
                var response = ApiResponse<TanqueDto>.ErrorResponse("Error interno del servidor");
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Actualiza el nivel de agua de un tanque mediante DTO
        /// </summary>
        [HttpPut("{id}/nivel")]
        public async Task<ActionResult<ApiResponse<TanqueDto>>> UpdateNivelTanque(int id, [FromBody] UpdateNivelTanqueDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var badRequestResponse = ApiResponse<TanqueDto>.ErrorResponse("Datos de entrada no válidos");
                    return BadRequest(badRequestResponse);
                }

                var tanque = await _tanqueService.UpdateNivelTanqueAsync(id, updateDto);
                var response = ApiResponse<TanqueDto>.SuccessResponse(tanque, $"Nivel del tanque actualizado a {updateDto.NivelAgua}%");

                _logger.LogInformation("Nivel del tanque {TanqueId} actualizado a {NuevoNivel}%", id, updateDto.NivelAgua);
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = ApiResponse<TanqueDto>.ErrorResponse($"Tanque with ID {id} not found");
                return NotFound(notFoundResponse);
            }
            catch (ArgumentException ex)
            {
                var badRequestResponse = ApiResponse<TanqueDto>.ErrorResponse(ex.Message);
                return BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el nivel del tanque con ID {TanqueId}", id);
                var response = ApiResponse<TanqueDto>.ErrorResponse("Error interno del servidor");
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Recibe señal directa del sensor y actualiza el nivel de agua (endpoint principal para señales de sensores)
        /// </summary>
        [HttpPost("{id}/senal-nivel")]
        public async Task<ActionResult<ApiResponse<HistorialNivelDto>>> RecibirSenalNivel(int id, [FromBody] double nivelAgua)
        {
            try
            {
                if (nivelAgua < 0 || nivelAgua > 100)
                {
                    var badRequestResponse = ApiResponse<HistorialNivelDto>.ErrorResponse("El nivel de agua debe estar entre 0 y 100");
                    return BadRequest(badRequestResponse);
                }

                var historial = await _tanqueService.RegistrarCambioNivelAsync(id, nivelAgua);
                var response = ApiResponse<HistorialNivelDto>.SuccessResponse(historial, "Señal de nivel procesada exitosamente");

                _logger.LogInformation("Señal recibida para tanque {TanqueId}: nivel {NuevoNivel}% (cambio: {TipoMovimiento})",
                    id, nivelAgua, historial.TipoMovimiento);

                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = ApiResponse<HistorialNivelDto>.ErrorResponse($"Tanque with ID {id} not found");
                return NotFound(notFoundResponse);
            }
            catch (ArgumentException ex)
            {
                var badRequestResponse = ApiResponse<HistorialNivelDto>.ErrorResponse(ex.Message);
                return BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar señal de nivel para tanque con ID {TanqueId}", id);
                var response = ApiResponse<HistorialNivelDto>.ErrorResponse("Error interno del servidor");
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Elimina un tanque
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteTanque(int id)
        {
            try
            {
                await _tanqueService.DeleteTanqueAsync(id);
                var response = ApiResponse<object>.SuccessResponse(null, "Tanque eliminado exitosamente");
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = ApiResponse<object>.ErrorResponse($"Tanque with ID {id} not found");
                return NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el tanque con ID {TanqueId}", id);
                var response = ApiResponse<object>.ErrorResponse("Error interno del servidor");
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Obtiene un resumen completo del estado de todos los tanques
        /// </summary>
        [HttpGet("resumen")]
        public async Task<ActionResult<ApiResponse<TanqueResumenDto>>> GetResumenTanques()
        {
            try
            {
                var resumen = await _tanqueService.GetResumenTanquesAsync();
                var response = ApiResponse<TanqueResumenDto>.SuccessResponse(resumen, "Resumen de tanques obtenido exitosamente");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el resumen de tanques");
                var response = ApiResponse<TanqueResumenDto>.ErrorResponse("Error interno del servidor");
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Obtiene solo los tanques activos
        /// </summary>
        [HttpGet("activos")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TanqueDto>>>> GetTanquesActivos()
        {
            try
            {
                var tanques = await _tanqueService.GetTanquesActivosAsync();
                var response = ApiResponse<IEnumerable<TanqueDto>>.SuccessResponse(tanques, "Tanques activos obtenidos exitosamente");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los tanques activos");
                var response = ApiResponse<IEnumerable<TanqueDto>>.ErrorResponse("Error interno del servidor");
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Obtiene tanques filtrados por estado de nivel (critico, bajo, medio, alto)
        /// </summary>
        [HttpGet("estado/{estadoNivel}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TanqueDto>>>> GetTanquesPorEstadoNivel(string estadoNivel)
        {
            try
            {
                var tanques = await _tanqueService.GetTanquesPorEstadoNivelAsync(estadoNivel);
                var response = ApiResponse<IEnumerable<TanqueDto>>.SuccessResponse(tanques, $"Tanques con nivel {estadoNivel} obtenidos exitosamente");
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                var badRequestResponse = ApiResponse<IEnumerable<TanqueDto>>.ErrorResponse(ex.Message);
                return BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tanques por estado de nivel {EstadoNivel}", estadoNivel);
                var response = ApiResponse<IEnumerable<TanqueDto>>.ErrorResponse("Error interno del servidor");
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Obtiene tanques con nivel crítico (menos del 20%)
        /// </summary>
        [HttpGet("nivel-critico")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TanqueDto>>>> GetTanquesNivelCritico()
        {
            try
            {
                var tanques = await _tanqueService.GetTanquesNivelCriticoAsync();
                var response = ApiResponse<IEnumerable<TanqueDto>>.SuccessResponse(tanques, "Tanques con nivel crítico obtenidos exitosamente");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tanques con nivel crítico");
                var response = ApiResponse<IEnumerable<TanqueDto>>.ErrorResponse("Error interno del servidor");
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Obtiene tanques por rango de nivel personalizado
        /// </summary>
        [HttpGet("rango-nivel")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TanqueDto>>>> GetTanquesPorRangoNivel([FromQuery] double nivelMinimo, [FromQuery] double nivelMaximo)
        {
            try
            {
                var tanques = await _tanqueService.GetTanquesPorRangoNivelAsync(nivelMinimo, nivelMaximo);
                var response = ApiResponse<IEnumerable<TanqueDto>>.SuccessResponse(tanques, $"Tanques con nivel entre {nivelMinimo}% y {nivelMaximo}% obtenidos exitosamente");
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                var badRequestResponse = ApiResponse<IEnumerable<TanqueDto>>.ErrorResponse(ex.Message);
                return BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tanques por rango de nivel {NivelMinimo}-{NivelMaximo}", nivelMinimo, nivelMaximo);
                var response = ApiResponse<IEnumerable<TanqueDto>>.ErrorResponse("Error interno del servidor");
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Endpoint simplificado para recibir solo el valor numérico del sensor
        /// </summary>
        [HttpPost("{id}/sensor")]
        public async Task<ActionResult<ApiResponse<TanqueDto>>> RecibirDatoSensor(int id, [FromQuery] double nivel)
        {
            try
            {
                if (nivel < 0 || nivel > 100)
                {
                    var badRequestResponse = ApiResponse<TanqueDto>.ErrorResponse("El nivel debe estar entre 0 y 100");
                    return BadRequest(badRequestResponse);
                }

                var tanque = await _tanqueService.UpdateNivelTanqueAsync(id, nivel);
                var response = ApiResponse<TanqueDto>.SuccessResponse(tanque, $"Datos del sensor procesados - Nivel: {nivel}%");

                _logger.LogInformation("Datos del sensor recibidos para tanque {TanqueId}: {Nivel}%", id, nivel);
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = ApiResponse<TanqueDto>.ErrorResponse($"Tanque with ID {id} not found");
                return NotFound(notFoundResponse);
            }
            catch (ArgumentException ex)
            {
                var badRequestResponse = ApiResponse<TanqueDto>.ErrorResponse(ex.Message);
                return BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar datos del sensor para tanque {TanqueId}", id);
                var response = ApiResponse<TanqueDto>.ErrorResponse("Error interno del servidor");
                return StatusCode(500, response);
            }
        }
    }
}